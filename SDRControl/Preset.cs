using System.Collections.Generic;
using System.IO;
using Humanizer;
using Newtonsoft.Json;

namespace SDRControl
{
    public class Preset
    {
        public string Id { get; set; }
        public string Name { get { return Id.Titleize(); }}
        public Queue<RemoteCommand> Commands = new Queue<RemoteCommand>();

        public Preset AddCommand(RemoteCommand command)
        {
            Commands.Enqueue(command);
            return this;
        }

        public static List<Preset> GetPresets(string pathToPresets)
        {
            //todo, cache these
            var files = Directory.GetFiles(pathToPresets);
            var presets = new List<Preset>();

            foreach (var file in files)
                presets.Add(new Preset { Id = Path.GetFileNameWithoutExtension(file) });

            return presets;
        }

        public static Queue<RemoteCommand> Find(string pathToPresets, string preset)
        {
            var str = File.ReadAllText(Path.Combine(pathToPresets, $"{preset}.json"));
            return JsonConvert.DeserializeObject<Queue<RemoteCommand>>(str);

            // switch (preset)
            // {
            //     case "radio one":
            //     case "radioone":
            //     case "radio1": return RadioOne();
            //     case "radiotwo":
            //     case "radio two":
            //     case "radio2": return RadioTwo();
            //     case "radiothree":
            //     case "radio three":
            //     case "radio3": return RadioThree();
            //     case "radiofour":
            //     case "radio four":
            //     case "radio4": return RadioFour();
            //     case "radiofivelive":
            //     case "radio five live":
            //     case "radio5live": return RadioFiveLive();
            //     case "radio ulster":
            //     case "radioulster": return RadioUlster();
            //     case "talksport": return Talksport();

            //     default: return new Queue<RemoteCommand>();
            // }
        }


        // public static Queue<RemoteCommand> RadioOne()
        // {

        //     var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
        //     var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "WFM");
        //     var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
        //     var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
        //     var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
        //     var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
        //     var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 99700000);

        //     var queue = new Preset().AddCommand(gainAudioCommand)
        //                 .AddCommand(detectorTypeCommand)
        //                 .AddCommand(filterBandwidthCommand)
        //                 .AddCommand(squelchEnabledCommand)
        //                 .AddCommand(SquelchThresholdCommand)
        //                 .AddCommand(fmStereoCommand)
        //                 .AddCommand(frequencyCommand);

        //     return queue.Commands;
        // }
        // public static Queue<RemoteCommand> RadioTwo()
        // {

        //     var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
        //     var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "WFM");
        //     var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
        //     var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
        //     var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
        //     var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
        //     var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 90200000);

        //     var queue = new Preset().AddCommand(gainAudioCommand)
        //                 .AddCommand(detectorTypeCommand)
        //                 .AddCommand(filterBandwidthCommand)
        //                 .AddCommand(squelchEnabledCommand)
        //                 .AddCommand(SquelchThresholdCommand)
        //                 .AddCommand(fmStereoCommand)
        //                 .AddCommand(frequencyCommand);

        //     return queue.Commands;
        // }
        // public static Queue<RemoteCommand> RadioThree()
        // {

        //     var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
        //     var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "WFM");
        //     var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
        //     var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
        //     var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
        //     var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
        //     var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 92600000);

        //     var queue = new Preset().AddCommand(gainAudioCommand)
        //                 .AddCommand(detectorTypeCommand)
        //                 .AddCommand(filterBandwidthCommand)
        //                 .AddCommand(squelchEnabledCommand)
        //                 .AddCommand(SquelchThresholdCommand)
        //                 .AddCommand(fmStereoCommand)
        //                 .AddCommand(frequencyCommand);

        //     return queue.Commands;
        // }
        // public static Queue<RemoteCommand> RadioFour()
        // {

        //     var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
        //     var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "WFM");
        //     var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
        //     var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
        //     var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
        //     var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
        //     var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 103000000);

        //     var queue = new Preset().AddCommand(gainAudioCommand)
        //                 .AddCommand(detectorTypeCommand)
        //                 .AddCommand(filterBandwidthCommand)
        //                 .AddCommand(squelchEnabledCommand)
        //                 .AddCommand(SquelchThresholdCommand)
        //                 .AddCommand(fmStereoCommand)
        //                 .AddCommand(frequencyCommand);

        //     return queue.Commands;
        // }
        // public static Queue<RemoteCommand> RadioFiveLive()
        // {

        //     var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
        //     var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "AM");
        //     var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
        //     var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
        //     var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
        //     var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
        //     var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 693000000);

        //     var queue = new Preset().AddCommand(gainAudioCommand)
        //                 .AddCommand(detectorTypeCommand)
        //                 .AddCommand(filterBandwidthCommand)
        //                 .AddCommand(squelchEnabledCommand)
        //                 .AddCommand(SquelchThresholdCommand)
        //                 .AddCommand(fmStereoCommand)
        //                 .AddCommand(frequencyCommand);

        //     return queue.Commands;
        // }
        // public static Queue<RemoteCommand> RadioUlster()
        // {

        //     var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
        //     var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "WFM");
        //     var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
        //     var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
        //     var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
        //     var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
        //     var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 92000000);

        //     var queue = new Preset().AddCommand(gainAudioCommand)
        //                 .AddCommand(detectorTypeCommand)
        //                 .AddCommand(filterBandwidthCommand)
        //                 .AddCommand(squelchEnabledCommand)
        //                 .AddCommand(SquelchThresholdCommand)
        //                 .AddCommand(fmStereoCommand)
        //                 .AddCommand(frequencyCommand);

        //     return queue.Commands;
        // }
        // public static Queue<RemoteCommand> Talksport()
        // {

        //     var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
        //     var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "AM");
        //     var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
        //     var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
        //     var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
        //     var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
        //     var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 1089000000);

        //     var queue = new Preset().AddCommand(gainAudioCommand)
        //                 .AddCommand(detectorTypeCommand)
        //                 .AddCommand(filterBandwidthCommand)
        //                 .AddCommand(squelchEnabledCommand)
        //                 .AddCommand(SquelchThresholdCommand)
        //                 .AddCommand(fmStereoCommand)
        //                 .AddCommand(frequencyCommand);

        //     return queue.Commands;
        // }
    }
}