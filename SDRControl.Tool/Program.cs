using McMaster.Extensions.CommandLineUtils;
using SDRControl;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace SDRControl.Tool
{
    class Program
    {
       public static async Task<int> Main(string[] args) => await CommandLineApplication.ExecuteAsync<Program>(args);

        [Argument(0, Description = "Command to run (Get, Exe etc")]
        [Required]
        public string Command { get; }

        [Option("-m|--method")]
        public string Method { get; }

        [Option("-v|--value")]
        public string Value { get; }

        [Option("-p|--preset")]
        public string PresetOption { get; }

        private async Task<int> OnExecute()
        {
            var config = new Config(); //todo: get from settings file
            config.PathToPresets = @"C:\Users\Steven\Source\SDRControl\SDRControl.Web\Presets";

            using (var remote = new SdrRemote(config))
            {
                switch (Command)
                {
                    case "start" : Console.WriteLine(await remote.Start()); break;
                    case "stop" : Console.WriteLine(await remote.Stop()); break;
                    case "isplaying" : Console.WriteLine(await remote.IsPlaying()); break;
                    case "play" : Console.WriteLine(await remote.Execute(Preset.Find(config.PathToPresets, PresetOption))); break;
                }
                //Console.ReadKey();
            }
           return 0;
        }
    }
}
