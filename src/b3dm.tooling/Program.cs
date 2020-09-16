using System;
using System.IO;
using System.Linq;
using B3dm.Tile;
using CommandLine;
using SharpGLTF.Schema2;
using SharpGLTF.Validation;

namespace b3dm.tooling
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<PackOptions, UnpackOptions, InfoOptions>(args).WithParsed(o =>
            {
                switch (o)
                {
                    case InfoOptions options:
                        Info(options);
                        break;
                    case PackOptions options:
                        Pack(options);
                        break;
                    case UnpackOptions options:
                        Unpack(options);
                        break;
                }
            });
        }

        static void Pack(PackOptions o)
        {
            Console.WriteLine($"Action: Pack");
            Console.WriteLine($"Input: {o.Input}");
            var f = File.ReadAllBytes(o.Input);
            var batchTableJsonFile = Path.GetFileNameWithoutExtension(o.Input) + ".batchtable.json";
            var featureTableJsonFile = Path.GetFileNameWithoutExtension(o.Input) + ".featuretable.json";
            var b3dm = new B3dm.Tile.B3dm(f);

            if (File.Exists(batchTableJsonFile))
            {
                Console.WriteLine($"Input batchtable json file: {batchTableJsonFile}");
                var batchTableJson = File.ReadAllLines(batchTableJsonFile);
                b3dm.FeatureTableJson = batchTableJson[0];
            }

            if (File.Exists(featureTableJsonFile))
            {
                Console.WriteLine($"Input featuretable json file: {featureTableJsonFile}");
                var featureTableJson = File.ReadAllLines(featureTableJsonFile);
                b3dm.FeatureTableJson = featureTableJson[0];
            }

            var b3dmfile = (o.Output == string.Empty ? Path.GetFileNameWithoutExtension(o.Input) + ".b3dm" : o.Output);

            if (File.Exists(b3dmfile) && !o.Force)
            {
                Console.WriteLine($"File {b3dmfile} already exists. Specify -f or --force to overwrite existing files.");
            }
            else
            {
                B3dmWriter.WriteB3dm(b3dmfile, b3dm);
                Console.WriteLine("B3dm created " + b3dmfile);
            }
        }

        static void Unpack(UnpackOptions o)
        {
            Console.WriteLine($"Action: Unpack");
            Console.WriteLine($"Input: {o.Input}");
            var f = File.OpenRead(o.Input);
            var b3dm = B3dmReader.ReadB3dm(f);
            Console.WriteLine("b3dm version: " + b3dm.B3dmHeader.Version);

            var glbfile = (o.Output == string.Empty ? Path.GetFileNameWithoutExtension(o.Input) + ".glb" : o.Output);
            var batchTableJsonFile = (o.Output == string.Empty ? Path.GetFileNameWithoutExtension(o.Input) + ".batchtable.json" : o.Output);
            var featureTableJsonFile = (o.Output == string.Empty ? Path.GetFileNameWithoutExtension(o.Input) + ".featuretable.json" : o.Output);

            if (File.Exists(glbfile) && !o.Force)
            {
                Console.WriteLine($"File {glbfile} already exists. Specify -f or --force to overwrite existing files.");
            }
            else
            {
                File.WriteAllBytes(glbfile, b3dm.GlbData);
                Console.WriteLine($"Glb created: {glbfile}");
                if (b3dm.BatchTableJson != String.Empty)
                {
                    File.WriteAllText(batchTableJsonFile, b3dm.BatchTableJson);
                    Console.WriteLine($"BatchTable json file created: {batchTableJsonFile}");
                }
                if (b3dm.FeatureTableJson != String.Empty)
                {
                    File.WriteAllText(featureTableJsonFile, b3dm.FeatureTableJson);
                    Console.WriteLine($"FeatureTable json file created: {featureTableJsonFile}");
                }

            }
        }

        static void Info(InfoOptions o)
        {
            Console.WriteLine($"Action: Info");
            Console.WriteLine("b3dm file: " + o.Input);
            var f = File.OpenRead(o.Input);
            var b3dm = B3dmReader.ReadB3dm(f);
            Console.WriteLine("b3dm header version: " + b3dm.B3dmHeader.Version);
            Console.WriteLine("b3dm header magic: " + b3dm.B3dmHeader.Magic);
            Console.WriteLine("b3dm header bytelength: " + b3dm.B3dmHeader.ByteLength);
            Console.WriteLine("b3dm header featuretablejson length: " + b3dm.B3dmHeader.FeatureTableJsonByteLength);
            Console.WriteLine("b3dm header batchtablejson length: " + b3dm.B3dmHeader.BatchTableJsonByteLength);
            Console.WriteLine("Feature table json: '" + b3dm.FeatureTableJson + "'");
            Console.WriteLine("Batch table json: '" + b3dm.BatchTableJson + "'");

            var validationErrors = b3dm.B3dmHeader.Validate();
            if (validationErrors.Count > 0)
            {
                Console.WriteLine($"Byte padding rule check: {validationErrors.Count} errors");
                foreach (var error in validationErrors)
                {
                    Console.WriteLine(error);
                }
            }
            else
            {
                Console.WriteLine("Validation check: no errors");
            }

            var stream = new MemoryStream(b3dm.GlbData);
            try
            {
                var glb = ModelRoot.ReadGLB(stream, new ReadSettings());
                Console.WriteLine("glTF model is loaded");
                Console.WriteLine("glTF generator: " + glb.Asset.Generator);
                Console.WriteLine("glTF version:" + glb.Asset.Version);
                Console.WriteLine("glTF primitives: " + glb.LogicalMeshes[0].Primitives.Count);
                var triangles = Toolkit.EvaluateTriangles(glb.DefaultScene).ToList();
                Console.WriteLine("glTF triangles: " +triangles.Count);

                var points = triangles.SelectMany(item => new[] { item.A.GetGeometry().GetPosition(), item.B.GetGeometry().GetPosition(), item.C.GetGeometry().GetPosition() }.Distinct().ToList());
                var xmin = (from p in points select p.X).Min();
                var xmax = (from p in points select p.X).Max();
                var ymin = (from p in points select p.Y).Min();
                var ymax = (from p in points select p.Y).Max();
                var zmin = (from p in points select p.Z).Min();
                var zmax = (from p in points select p.Z).Max();

                Console.WriteLine($"Bounding box vertices: {xmin}, {xmax}, {ymin}, {ymax}, {zmin}, {zmax}");
                foreach (var primitive in glb.LogicalMeshes[0].Primitives)
                {
                    Console.Write($"Primitive {primitive.LogicalIndex} ({primitive.DrawPrimitiveType}) ");

                    if (primitive.GetVertexAccessor("_BATCHID") != null)
                    {
                        var batchIds = primitive.GetVertexAccessor("_BATCHID").AsScalarArray();
                        Console.WriteLine($"batch ids (unique): {string.Join(',',batchIds.Distinct())}");

                    }
                    else
                    {
                        Console.WriteLine($"No _BATCHID attribute found...");
                    }
                }

                if (glb.ExtensionsUsed.Count() > 0)
                {
                    Console.WriteLine("glTF extensions used: " + string.Join(',', glb.ExtensionsUsed));
                }
                else
                {
                    Console.WriteLine("glTF: no extensions used.");
                }
                if (glb.ExtensionsRequired.Count() > 0)
                {
                    Console.WriteLine("glTF extensions required: " + string.Join(',', glb.ExtensionsRequired));
                }
                else
                {
                    Console.WriteLine("glTF: no extensions required.");
                }

            }
            catch (SchemaException ex)
            {
                Console.WriteLine("glTF schema exception");
                Console.WriteLine(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Invalid data exception");
                Console.WriteLine(ex.Message);
            }
            catch (LinkException ex)
            {
                Console.WriteLine("glTF Link exception");
                Console.WriteLine(ex.Message);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            f.Dispose();
        }
    }
}
