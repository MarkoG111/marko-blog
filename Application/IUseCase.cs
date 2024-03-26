using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public interface IUseCase
    {
        int Id { get; }
        string Name { get; }
    }

    public interface ICommand<TRequest> : IUseCase
    {
        void Execute(TRequest request);
    }

    public interface IQuery<TResponse, TSearch> : IUseCase
    {
        TResponse Execute(TSearch search);
    }

    public interface IQueryWithoutSearch<TResponse> : IUseCase
    {
        TResponse Execute();
    }
}