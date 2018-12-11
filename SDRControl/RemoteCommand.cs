using System;
using System.Threading.Tasks;
using PrimS.Telnet;
using Newtonsoft.Json;

namespace SDRControl
{
    public class RemoteCommand
    {
        public string Command { get; set; }
        public string Method { get; set; }
        public object Value { get; set; }

        public static RemoteCommand Create(string type, string method, object value) => new RemoteCommand { Command = type, Method = method, Value = value };
        public static RemoteCommand Create(string type, string method) => new RemoteCommand { Command = type, Method = method };
    }
}