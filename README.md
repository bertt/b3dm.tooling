# b3dm.tooling

Global tooling for handling 3D Tiles b3dm files. 

Supports operations like:

- getting information about the b3dm (info);

- unpacking from b3dm to glb (unpack);

- creating b3dm from glb file (pack).

## API

Verbs:

```
  pack       pack b3dm

  unpack     unpack b3dm

  info       info b3dm

  help       Display more information on a specific command.

  version    Display version information.
```

Info options:

```
  -i, --input    Required. Input path of the .b3dm
```

Pack options:

```
  -i, --input     Required. Input path of the glb file

  -o, --output    (Default: ) Output path of the resulting .b3dm

  -f, --force     (Default: false) force overwrite output file
```

Batch information is retrieved from files .batchtable.json and .featuretable.json when present.


Unpack options:

```
  -i, --input     Required. Input path of the .b3dm

  -o, --output    (Default: ) Output path of the resulting .glb

  -f, --force     (Default: false) force overwrite output file
```

Batch information is saved in files .batchtable.json and .featuretable.json when present.

## Installation

Requirement: Install .NET SDK 6.0 https://dotnet.microsoft.com/download

- Install from NuGet

https://www.nuget.org/packages/b3dm.tooling/

```
$ dotnet tool install -g b3dm.tooling
```

or update:

```
$ dotnet tool update -g b3dm.tooling

```

## Running

1] Command Info b3dm_file gives header info about b3dm file and glTF in the payload

Example:

```
$ b3dm info -i test.b3dm

b3dm header version: 1
b3dm header magic: b3dm
b3dm header bytelength: 69658
b3dm header featuretablejson length: 20
b3dm header batchtablejson length: 521
Batch table json: {"hoehe":["17.386000000000024","18.34499999999997","18.58699999999999","21.860000000000014","10.168000000000006","20.584000000000003","19.70599999999996","19.817000000000007","20.000999999999976","16.577999999999975","17.865999999999985","17.745000000000005"],"citygml_class":["BB01","BB01","BB01","BB01","BB01","BB01","BB01","BB01","BB01","BB01","BB01","BB01"],"surfaceType":["roof","roof","roof","roof","roof","roof","roof","roof","roof","roof","roof","roof"],"Region":["5","5","5","5","5","5","5","5","5","5","5","5"]}
Feature table json: {"BATCH_LENGTH":12}
glTF model is loaded
Validation check: no errors
glTF model is loaded
glTF generator: Khronos glTF Blender I/O v1.2.75
glTF version:2.0
glTF primitives: 1
glTF triangles: 722
Bounding box vertices: -0.2226839, 0.2226839, 0, 0.56007576, -0.26161957, 0.26161957
```

2] Command unpack -i b3dm_filename 

unpacks a b3dm file to GLB format and creates {name}.batchtable.json and {name}.featuretable.json files when containing 
batchTableJson information

Example:

```
$ b3dm unpack -i 1.b3dm
Action: Unpack
Input: 1.b3dm
b3dm version: 1
glTF asset generator: py3dtiles
glTF version: 2.0
Buffer bytes: 167832
Glb created: 1.glb
BatchTable json file created: 1.batchtable.json
FeatureTable json file created: 1.featuretable.json
```

Sample file 1.batchtable.json:

```
{"id":["10103","800","2117","2497","11214","7076","4140","9584","784","9294","9295","7075","7078","20240","2116","7077","13523","6131","11300","13466","12805","7074","4411","7079","6443","2786","7073","7072","1200","11281","2115"]}
```

Sample file 1.featuretable.json:

```
{"BATCH_LENGTH":31}
```

3] Command pack -i glb_filename to pack a glb to b3dm file and importing batchTableJson when .batch file exists.

Example:

```
$ b3dm pack -i 1.glb

Action: Pack
Input: 1.b3dm
Input batch file: 1.batchtable.json
B3dm created 1.b3dm
```

## Building from source

```
$ cd b3dm-tile-cs\b3dm.tooling
$ dotnet pack
$ dotnet tool install --global --add-source ./nupkg b3dm.tooling
```

or update:

```
$ dotnet tool update --global --add-source ./nupkg b3dm.tooling
```

## History

2024-01-25: release 1.0.5 - bug fix reading glb from b3dm with trailing padding

2023-02-16: release 1.0.3, 1.0.4 - bug fix reading fglb from b3dm with trailing padding

2023-02-16: release 1.0.1, 1.0.2 - bug fix for option pack with remote file

2022-12-08: release 1.0 - upgrading to .NET 6

2020-09-16: simplify unpack - release 0.20

2020-08-21: adding reading/writing batch information to/from .batchtable.json and .featuretable.json

2020-08-19: added support for reading glTF with KHR_mesh_quantization. 
A custom build of SharpGltf on myget is used for now: https://www.myget.org/feed/bertt/package/nuget/SharpGLTF.Toolkit/1.0.0-Preview-20200819-1221



