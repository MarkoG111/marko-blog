using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class AlreadyAddedException : Exception
    {
        public AlreadyAddedException(IApplicationActor actor)
            : base($"Actor with an ID of {actor.Id} - {actor.Identity} already submitted request to admin approval.")
        {

        }
    }
}