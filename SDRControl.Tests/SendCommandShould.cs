using System;
using Xunit;
using PrimS.Telnet;
using Newtonsoft.Json;
using Shouldly;
using System.Threading.Tasks;

namespace SDRControl.Tests
{
    public class SdrRemoteShould
    {
        public Config Config { get; }
        public SdrRemoteShould()
        {
            Config = new Config { IpAddress = "localhost", Port = 3382 };
        }

        [Fact]
        public async Task ConnectAndSendCommandAsync()
        {
            using (var remote = new SdrRemote(Config))
            {
                remote.IsConnected().ShouldBeTrue();


                var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
                var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "WFM");
                var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
                var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
                var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
                var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
                var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 99700000);

                await remote.Stop();

                var gainAudioResponse = await remote.SendAsync(gainAudioCommand);
                Console.WriteLine(gainAudioResponse);
                var detectorTypeResponse = await remote.SendAsync(detectorTypeCommand);
                Console.WriteLine(detectorTypeResponse);
                var filterBandwidthResponse = await remote.SendAsync(filterBandwidthCommand);
                Console.WriteLine(filterBandwidthResponse);
                var squelchEnabledResponse = await remote.SendAsync(squelchEnabledCommand);
                Console.WriteLine(squelchEnabledResponse);
                var SquelchThresholdResponse = await remote.SendAsync(SquelchThresholdCommand);
                Console.WriteLine(SquelchThresholdResponse);
                var fmStereoResponse = await remote.SendAsync(fmStereoCommand);
                Console.WriteLine(fmStereoResponse);
                var frequencyResponse = await remote.SendAsync(frequencyCommand);
                Console.WriteLine(frequencyResponse);

                await remote.Start();
            }
        }

        [Fact]
        public async Task ConnectAndSendCommandsUsingBuilderAsync()
        {
            using (var remote = new SdrRemote(Config))
            {
                remote.IsConnected().ShouldBeTrue();

                var gainAudioCommand = RemoteCommand.Create("Set", "AudioGain", 45);
                var detectorTypeCommand = RemoteCommand.Create("Set", "DetectorType", "WFM");
                var filterBandwidthCommand = RemoteCommand.Create("Set", "FilterBandwidth", 200000);
                var squelchEnabledCommand = RemoteCommand.Create("Set", "SquelchEnabled", false);
                var SquelchThresholdCommand = RemoteCommand.Create("Set", "SquelchThreshold", 30);
                var fmStereoCommand = RemoteCommand.Create("Set", "FmStereo", true);
                var frequencyCommand = RemoteCommand.Create("Set", "Frequency", 99700000);

                await remote.AddCommand(gainAudioCommand)
                              .AddCommand(detectorTypeCommand)
                              .AddCommand(filterBandwidthCommand)
                              .AddCommand(squelchEnabledCommand)
                              .AddCommand(SquelchThresholdCommand)
                              .AddCommand(fmStereoCommand)
                              .AddCommand(frequencyCommand)
                              .Execute();

                remote.ResponseLog.Count.ShouldBeGreaterThan(10);
                remote.ToString().ShouldNotBeNullOrWhiteSpace();
            }
        }
        [Fact]
        public async Task GetCurrentState()
        {
            using (var remote = new SdrRemote(Config))
            {
                remote.IsConnected().ShouldBeTrue();

               var state = await remote.CurrentState();

               state.AudioGain.ShouldNotBeNullOrEmpty();
               state.DetectorType.ShouldNotBeNullOrEmpty();
               state.FmStereo.ShouldNotBeNullOrEmpty();
            }
        }
    }
}
