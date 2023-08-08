using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Vertification.EmailVertification
{
    public interface IEmailAuthenticatorHelper
    {
        public Task<string> CreateEmailActivationKey();
        public Task<string> CreateEmailActivationCode();
    }
}
