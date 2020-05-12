using CommandLine;

namespace b3dm.tooling
{
    [Verb("pack", HelpText = "pack glb to b3dm")]
    public class PackOptions
    { 
        [Option('i', "input", Required = true, HelpText = "Input path of the glb file")]
        public string Input { get; set; }
        [Option('o', "output", Required = false, Default = "", HelpText = "Output path of the resulting .b3dm")]
        public string Output { get; set; }
        [Option('f', "force", Required = false, Default = false, HelpText = "force overwrite output file")]
        public bool Force { get; set; }
    }

    [Verb("unpack", HelpText = "unpack b3dm to glb")]
    public class UnpackOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input path of the .b3dm")]
        public string Input { get; set; }
        [Option('o', "output", Required = false, Default = "", HelpText = "Output path of the resulting .glb")]
        public string Output { get; set; }
        [Option('f', "force", Required = false, Default = false, HelpText = "force overwrite output file")]
        public bool Force { get; set; }
    }

    [Verb("info", HelpText = "info b3dm")]
    public class InfoOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input path of the .b3dm")]
        public string Input { get; set; }
    }
}
