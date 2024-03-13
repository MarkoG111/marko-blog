using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public interface IUseCaseLogger
    {
        void Log(IUseCase useCase, IApplicationActor actor, object data);
    }
}