using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Likes;
using Domain;

namespace Application.Services
{
    public interface ILikeService
    {
        Task<Like> ToggleLike(LikeDto request);
    }
}