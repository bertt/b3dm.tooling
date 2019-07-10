# b3dm.tooling

Global tooling for handling b3dm files, like getting information about the b3dm (info), unpacking to glb (unpack) or creating b3dm from glb file (pack).

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
$ b3dm info test.b3dm
```

2] Command unpack b3dm_filename unpacks a b3dm file to GLB format

Example:

```
$ b3dm unpack test.b3dm

b3dm version: 1
glTF asset generator: py3dtiles
glTF version: 2.0
Buffer bytes: 1848
Glb created test.glb
```

3] Command pack glb_filename to pack a glb to b3dm file

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



