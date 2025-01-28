using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Searches;
using Application.DataTransfer.UseCases;

namespace Application.Queries.UseCaseLogs
{
    public interface IGetUseCaseLogsQuery : IQuery<PagedResponse<GetUseCaseLogDto>, UseCaseLogSearch>
    {

    }
}