using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SDRControl;

namespace SDRControl.Web.Pages
{
    public class IndexModel : PageModel
    {
        public bool isPlaying { get; set; }
        public string PlayButtonText { get { return isPlaying ? "Stop" : "Play"; } }
        public string PlayButtonStyle { get { return isPlaying ? "btn btn-primary" : "btn btn-success"; } }
        public string PlayButtonIcon { get { return isPlaying ? "fas fa-stop" : "fas fa-play"; } }
        public SelectList DetectorTypes { get; set; }
        public SdrState CurrentState { get; set; }
        public string Logs { get { return _remote != null ? _remote.ToString() : "No logs generated."; } }
        public List<Preset> Presets { get { return Preset.GetPresets(_config.PathToPresets); } }
        private readonly ISdrRemote _remote;
        private readonly Config _config;
        public IndexModel(ISdrRemote remote, Config config)
        {
            _remote = remote;
            _config = config;
        }

        public async Task OnGet()
        {
            await Load();
        }

        //need to use some caching here as Load() is very chatty
        private async Task Load()
        {
            PopulateTypes();
            isPlaying = await _remote.IsPlaying();
            CurrentState = await _remote.CurrentState();
        }

        public async Task OnPostSetPreset(string station)
        {
            await Load();

            await _remote.Execute(Preset.Find(_config.PathToPresets, station));

            await Load();
        }
        public  async Task OnPostTogglePlayer()
        {
            await Load();

            if(isPlaying)
                await _remote.Stop();
            else
                await _remote.Start();

            await Load();
        }

        public  async Task OnPostManualConfig()
        {
                var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", CurrentState.AudioGain);
                var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", CurrentState.DetectorType);
                var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
                var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
                var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
                var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
                var frequencyCommand = RemoteCommand.Create("Set", "Frequency", CurrentState.Frequency);

                await _remote.AddCommand(gainAudioCommand)
                              .AddCommand(detectorTypeCommand)
                              .AddCommand(filterBandwidthCommand)
                              .AddCommand(squelchEnabledCommand)
                              .AddCommand(SquelchThresholdCommand)
                              .AddCommand(fmStereoCommand)
                              .AddCommand(frequencyCommand)
                              .Execute();
        }

        private void PopulateTypes() => DetectorTypes = new SelectList(_remote.DetectorTypes());
    }
}
