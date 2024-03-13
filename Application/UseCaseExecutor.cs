using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public class UseCaseExecutor
    {
        private readonly IApplicationActor _actor;
        private readonly IUseCaseLogger _logger;

        public UseCaseExecutor(IApplicationActor actor, IUseCaseLogger logger)
        {
            _actor = actor;
            _logger = logger;
        }

        public void ExecuteCommand<TRequest>(ICommand<TRequest> command, TRequest data)
        {
            _logger.Log(command, _actor, data);

            command.Execute(data);
        }

        public TResponse ExecuteQuery<TResponse, TSearch>(IQuery<TResponse, TSearch> query, TSearch data)
        {
            _logger.Log(query, _actor, data);

            return query.Execute(data);
        }
    }
}