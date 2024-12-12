using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class AlreadyAddedException : Exception
    {
        public AlreadyAddedException(IApplicationActor actor)
            : base($"You already submitted request to admin approval")
        {

        }
    }
}