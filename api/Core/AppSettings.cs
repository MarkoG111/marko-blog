using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Core
{
    public class AppSettings
    {
        public string JwtSecretKey { get; set; }
        public string JwtIssuer { get; set; }
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public string ApplicationInstance { get; set; }
    }
}