using System;
using System.Threading.Tasks;
using PrimS.Telnet;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SDRControl
{
    public class SdrState
    {
        public string AudioGain { get; set; }
        public string DetectorType { get; set; }
        public string FilterBandwidth { get; set; }
        public string SquelchEnabled { get; set; }
        public string SquelchThreshold { get; set; }
        public string FmStereo { get; set; }
        public string Frequency { get; set; }
    }
}