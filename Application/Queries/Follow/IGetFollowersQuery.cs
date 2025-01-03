using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Followers;
using Application.Searches;

namespace Application.Queries.Follow
{
    public interface IGetFollowersQuery : IQuery<PagedResponse<GetFollowsDto>, int>
    {

    }
}