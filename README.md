# b3dm.tooling

Global tooling for handling b3dm files, like getting information about the b3dm (info), unpacking to glb (unpack) or creating b3dm from glb file (pack).

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


Unpack options:

```
  -i, --input     Required. Input path of the .b3dm

  -o, --output    (Default: ) Output path of the resulting .glb

  -f, --force     (Default: false) force overwrite output file
```


## Installation

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

1] Command Info b3dm_file gives header info about b3dm file

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
```

2] Command unpack -i b3dm_filename unpacks a b3dm file to GLB format

Example:

```
$ b3dm unpack -i test.b3dm

b3dm version: 1
glTF asset generator: py3dtiles
glTF version: 2.0
Buffer bytes: 1848
Glb created test.glb
```

3] Command pack -i glb_filename to pack a glb to b3dm file

Example:

```
$ b3dm pack test.glb

B3dm created test.b3dm
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



