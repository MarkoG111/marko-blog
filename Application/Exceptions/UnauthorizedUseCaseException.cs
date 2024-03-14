using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class UnauthorizedUseCaseException : Exception
    {
        public UnauthorizedUseCaseException(IUseCase useCase, IApplicationActor actor)
            : base($"Actor with an ID of {actor.Id} - {actor.Identity} tried to execute {useCase.Name}")
        {

        }
    }
}