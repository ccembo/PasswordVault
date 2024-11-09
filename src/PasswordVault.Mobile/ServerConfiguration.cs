using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordVault.Mobile
{
    internal class ServerConfiguration
    {
        public string Version { get; set; } = "1.0";
        public string Server { get; set; } = "http://192.168.1.192:5272/KeyExchangeHub";
        public string ServersPubKey { get; set; } = "ADBWDBILWBDIBWIUDBNJWNDJWNDWDMWPD==";
        public string user { get; set; } = "user";
        public string userPublickey { get; set; } = "";
        public string userPrivatekey { get; set; } = "";

    }
}
