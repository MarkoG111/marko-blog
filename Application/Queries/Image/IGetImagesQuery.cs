using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Application.DataTransfer;
using Application.Searches;

namespace Application.Queries.Image
{
    public interface IGetImagesQuery : IQuery<PagedResponse<GetImageDto>, ImageSearch>
    {

    }
}