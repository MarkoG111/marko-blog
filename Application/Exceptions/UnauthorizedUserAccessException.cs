using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class UnauthorizedUserAccessException : Exception
    {
        public UnauthorizedUserAccessException(IApplicationActor actor, string UseCaseName)
            : base($"User with identity: {actor.Identity} with ID: {actor.Id} has tried to execute Use Case {UseCaseName}")
        {

        }
    }
}