using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using Application.DataTransfer.Likes;
using Application.Services;
using Application.Repositories;

namespace Implementation.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<Domain.Like> ToggleLike(LikeDto request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var existingLike = await _likeRepository.GetLike(request.IdUser, request.IdPost, request.IdComment);

            if (existingLike != null)
            {
                if (existingLike.Status == request.Status)
                {
                    await _likeRepository.RemoveLike(existingLike);
                    await _likeRepository.SaveChangesAsync();
                    return null; // Indikator da je glas uklonjen
                }
                else
                {
                    existingLike.Status = request.Status;
                    await _likeRepository.SaveChangesAsync();
                    return existingLike;
                }
            }
            else
            {
                existingLike = new Like
                {
                    IdUser = request.IdUser,
                    IdPost = request.IdPost,
                    IdComment = request.IdComment,
                    Status = request.Status,
                    CreatedAt = DateTime.UtcNow
                };

                await _likeRepository.AddLike(existingLike);
            }

            await _likeRepository.SaveChangesAsync();
            return existingLike;
        }

        public async Task RemoveLike(LikeDto request)
        {
            var existingLike = await _likeRepository.GetLike(request.IdUser, request.IdPost, request.IdComment);

            if (existingLike != null)
            {
                await _likeRepository.RemoveLike(existingLike);
                await _likeRepository.SaveChangesAsync();
            }
        }
    }
}