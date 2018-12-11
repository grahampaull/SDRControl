using System;
using System.Threading.Tasks;
using PrimS.Telnet;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SDRControl
{
    public interface ISdrRemote
    {
        bool IsConnected();
        Task<string> Start();
        Task<string> Stop();
        Task<bool> IsPlaying();
        Task<string> SendAsync(RemoteCommand command);
        SdrRemote AddCommand(RemoteCommand command);
        Task<SdrRemote> Execute();
        Task<SdrRemote> Execute(Queue<RemoteCommand> commands);
        Task<SdrState> CurrentState();
        string[] DetectorTypes();
    }

    public class SdrRemote : ISdrRemote, IDisposable
    {
        private readonly Config _config;
        private Client _client;
        public List<string> ResponseLog { get; private set; } = new List<string>();
        private Queue<RemoteCommand> Commands = new Queue<RemoteCommand>();
        private bool _shouldStopAndStart = true;

        public SdrRemote(Config config)
        {
            _config = config;

            Connect();
        }

        private SdrRemote Connect()
        {
             _client = new Client(_config.IpAddress, _config.Port, new System.Threading.CancellationToken());
            return this;
        }
        public bool IsConnected() => _client != null && _client.IsConnected;
        public void Dispose() =>  _client.Dispose();

        public SdrRemote AddCommand(RemoteCommand command)
        {
            Commands.Enqueue(command);
            return this;
        }

        public async Task<SdrRemote> Execute(Queue<RemoteCommand> commands)
        {
            Commands = commands;
            await Execute();
            return this;
        }

        public async Task<SdrRemote> Execute()
        {
            _shouldStopAndStart = await IsPlaying();

            await Stop();

            while(Commands.Count > 0)
            {
                RemoteCommand command = Commands.Dequeue(); 
                await SendAsync(command);
            }

            await Start();

            return this;
        }

        public string[] DetectorTypes() => new string[] { "NFM", "AM", "LSB", "USB", "WFM", "DSB", "CW", "RAW" };

        public async Task<SdrState> CurrentState()
        {
            var gainAudioCommand = RemoteCommand.Create("Get", "AudioGain");
            var detectorTypeCommand = RemoteCommand.Create("Get", "DetectorType");
            var filterBandwidthCommand = RemoteCommand.Create("Get", "FilterBandwidth");
            var squelchEnabledCommand = RemoteCommand.Create("Get", "SquelchEnabled");
            var SquelchThresholdCommand = RemoteCommand.Create("Get", "SquelchThreshold");
            var fmStereoCommand = RemoteCommand.Create("Get", "FmStereo");
            var frequencyCommand = RemoteCommand.Create("Get", "Frequency");

            var gainAudioResponse = await SendAsync(gainAudioCommand);
            gainAudioResponse.TryParseJson<RemoteResult>(out var gainAudioValue);

            var detectorTypeResponse = await SendAsync(detectorTypeCommand);
            detectorTypeResponse.TryParseJson<RemoteResult>(out var detectorTypeValue);

            var filterBandwidthResponse = await SendAsync(filterBandwidthCommand);
            filterBandwidthResponse.TryParseJson<RemoteResult>(out var filterBandwidthValue);

            var squelchEnabledResponse = await SendAsync(squelchEnabledCommand);
            squelchEnabledResponse.TryParseJson<RemoteResult>(out var squelchEnabledValue);
            
            var SquelchThresholdResponse = await SendAsync(SquelchThresholdCommand);
            SquelchThresholdResponse.TryParseJson<RemoteResult>(out var squelchThresholdValue);

            var fmStereoResponse = await SendAsync(fmStereoCommand);
            fmStereoResponse.TryParseJson<RemoteResult>(out var fmStereoValue);
            
            var frequencyResponse = await SendAsync(frequencyCommand);
            frequencyResponse.TryParseJson<RemoteResult>(out var frequencyValue);

            return new SdrState { 
                AudioGain = gainAudioValue?.Value?.ToString(),  
                DetectorType = detectorTypeValue?.Value?.ToString(),  
                FilterBandwidth = filterBandwidthValue?.Value?.ToString(),  
                SquelchEnabled = squelchEnabledValue?.Value?.ToString(),  
                SquelchThreshold = squelchThresholdValue?.Value?.ToString(),  
                FmStereo = fmStereoValue?.Value?.ToString(), 
                Frequency = frequencyValue?.Value?.ToString()
            };
        }

        public async Task<bool> IsPlaying()
        {
            var response = await SendAsync(RemoteCommand.Create("Get", nameof(IsPlaying)));

            //SDR may wite back multiple objects from SDR on new lines, so we have to split and find the correct object
            var responses = response.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string str in responses)
            {
                bool isValidObject = str.TryParseJson<RemoteResult>(out var result);
                if(isValidObject && result.Method == nameof(IsPlaying))
                    return Convert.ToBoolean(result.Value);
            }
            return false;
        }

        private void LogResults(string response)
        {
            var responses = response.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string str in responses)
            {
                bool isValidObject = str.TryParseJson<RemoteResult>(out var result);
                if(isValidObject)
                    ResponseLog.Add($"Command Response: {result?.Result}" + (result?.Value != null ? $" with value {result.Value}" : ""));
                    
            }
        }

        public async Task<string> SendAsync(RemoteCommand command)
        {
            var cmd = JsonConvert.SerializeObject(command);
            
            ResponseLog.Add($"Sending Command: {command.Command} {command.Method} {(command.Value == null ? "": $"with value: {command.Value}")}");

            await _client.WriteLine(cmd);

            var response = await _client.ReadAsync(TimeSpan.FromMilliseconds(_config.TimeoutMs));

            LogResults(response);

            return response;
        }

        public async Task<string> Start()
        {
            var response = "Skipped Start";
            if(_shouldStopAndStart)
               response = await SendAsync(RemoteCommand.Create("Exe", "Start", true));

            ResponseLog.Add(response);

            return response;
        }

        public async Task<string> Stop()
        {
            var response = "Skipped Stop";
            if(_shouldStopAndStart)
               response = await SendAsync(RemoteCommand.Create("Exe", "Stop", true));

            ResponseLog.Add(response);
            
            return response;
        }

        public override string ToString()
        {
            var reversedList = ResponseLog.ToList();
            reversedList.Reverse();
            return string.Join(Environment.NewLine, reversedList);
        }
    }
}