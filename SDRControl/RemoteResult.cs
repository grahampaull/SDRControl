using System;
using System.Threading.Tasks;
using PrimS.Telnet;
using Newtonsoft.Json;

namespace SDRControl
{
    public class RemoteResult
    {
        public string Result { get; set; }
        public string Method { get; set; }
        public object Value { get; set; }

    }
}