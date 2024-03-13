using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;

namespace api.Core
{
    public class JWTActor : IApplicationActor
    {
        public int Id { get; set; }
        public string Identity { get; set; }
        public IEnumerable<int> AllowedUseCases { get; set; }
    }
}