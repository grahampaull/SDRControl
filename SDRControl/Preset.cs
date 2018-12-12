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

        }

    }
}