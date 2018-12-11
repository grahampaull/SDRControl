using System.IO;

namespace SDRControl
{

    public class Config
    {
        public string IpAddress { get; set; } = "localhost";
        public int Port { get; set; } = 3382;
        public int TimeoutMs { get; set; } = 100;
        public string PathToPresets { get; set; } = Directory.GetCurrentDirectory() + "/Presets";
    }
}