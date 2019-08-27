using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using B3dm.Tile;
using glTFLoader;
using glTFLoader.Schema;

namespace b3dm.tooling
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                var versionString = Assembly.GetEntryAssembly()
                                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                        .InformationalVersion
                                        .ToString();

                Console.WriteLine($"b3dm v{versionString}");
                Console.WriteLine("-------------");
                Console.WriteLine("\nUsage:");
                Console.WriteLine("  b3dm unpack <b3dm-file>");
                Console.WriteLine("  b3dm pack <glb-file>");
                Console.WriteLine("  b3dm info <b3dm-file>");
                return;
            }

            if (args[0] == "unpack") {
                Unpack(args[1]);
            }
            else if (args[0] == "pack") {
                Pack(args[1]);
            }
            else if (args[0] == "info") {
                Info(args[1]);
            }
        }

        static void Pack(string file)
        {
            var f = File.ReadAllBytes(file);
            var b3dm = new B3dm.Tile.B3dm(f);
            var b3dmfile = Path.GetFileNameWithoutExtension(file) + ".b3dm";
            B3dmWriter.WriteB3dm(b3dmfile, b3dm);
            Console.WriteLine("B3dm created " + b3dmfile);
        }

        static void Unpack(string file)
        {
            var f = File.OpenRead(file);
            var b3dm = B3dmReader.ReadB3dm(f);
            Console.WriteLine("b3dm version: " + b3dm.B3dmHeader.Version);
            var stream = new MemoryStream(b3dm.GlbData);
            try {
                var gltf = Interface.LoadModel(stream);
                Console.WriteLine("glTF asset generator: " + gltf.Asset.Generator);
                Console.WriteLine("glTF version: " + gltf.Asset.Version);
                var bufferBytes = gltf.Buffers[0].ByteLength;
                Console.WriteLine("Buffer bytes: " + bufferBytes);
                var glbfile = Path.GetFileNameWithoutExtension(file) + ".glb";
                File.WriteAllBytes(glbfile, b3dm.GlbData);
                Console.WriteLine("Glb created " + glbfile);
            }
            catch (InvalidDataException ex) {
                Console.WriteLine("glTF version not supported.");
                Console.WriteLine(ex.Message);
            }
        }

        static void Info(string file)
        {
            Console.WriteLine("b3dm file: " + file);
            var f = File.OpenRead(file);
            var b3dm = B3dmReader.ReadB3dm(f);
            Console.WriteLine("b3dm header version: " + b3dm.B3dmHeader.Version);
            Console.WriteLine("b3dm header magic: " + b3dm.B3dmHeader.Magic);
            Console.WriteLine("b3dm header bytelength: " + b3dm.B3dmHeader.ByteLength);
            Console.WriteLine("b3dm header featuretablejson length: " + b3dm.B3dmHeader.FeatureTableJsonByteLength);
            Console.WriteLine("b3dm header batchtablejson length: " + b3dm.B3dmHeader.BatchTableJsonByteLength);
            Console.WriteLine("Batch table json: " + b3dm.BatchTableJson);
            Console.WriteLine("Feature table json: " + b3dm.FeatureTableJson);
            var stream = new MemoryStream(b3dm.GlbData);
            try
            {
                var gltf = Interface.LoadModel(stream);
                Console.WriteLine("glTF model is loaded");
                Console.WriteLine("glTF generator: " + gltf.Asset.Generator);
                Console.WriteLine("glTF version:" + gltf.Asset.Version);
                Console.WriteLine("glTF primitives: " + gltf.Meshes[0].Primitives.Length);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("glTF version not supported.");
                Console.WriteLine(ex.Message);
            }
            f.Dispose();
        }
    }
}
