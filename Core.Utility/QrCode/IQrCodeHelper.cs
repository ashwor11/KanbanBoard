using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility.QrCode
{
    internal interface IQrCodeHelper
    {
        public string CreateQrCode(string message);
        public string CreateTotpQrCode(string secret, string issuer, string label);
    }
}
