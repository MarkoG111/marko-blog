using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Exceptions;
using Application.Commands.Like;
using Application.DataTransfer.Likes;
using Application.DataTransfer.Notifications;
using Application.Services;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Like;
using Implementation.Services;
using Domain;

namespace Implementation.Commands.Like
{
    public class EFUnlikePostCommand : IUnlikePostCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;
        private readonly ILikeService _likeService;

        public EFUnlikePostCommand(BlogContext context, IApplicationActor actor, ILikeService likeService)
        {
            _context = context;
            _actor = actor;
            _likeService = likeService;
        }

        public int Id => (int)UseCaseEnum.EFUnlikePostCommand;
        public string Name => UseCaseEnum.EFUnlikePostCommand.ToString();

        public async Task ExecuteAsync(LikeDto request)
        {
            if (request.IdPost == null)
            {
                throw new ArgumentException("IdPost is required.");
            }

            var post = await _context.Posts.FindAsync(request.IdPost);
            if (post == null)
            {
                throw new EntityNotFoundException(request.IdPost, typeof(Domain.Post));
            }

            await _likeService.RemoveLike(request);

            await _context.SaveChangesAsync();
        }
    }
}