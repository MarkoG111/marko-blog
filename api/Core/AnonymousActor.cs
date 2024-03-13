using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;

namespace api.Core
{
    public class AnonymousActor : IApplicationActor
    {
        public int Id => 0;
        public string Identity => "Unauthorized user";
        public IEnumerable<int> AllowedUseCases => new List<int> { 1, 5, 6, 13 };
    }
}