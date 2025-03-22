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
    public class EFUnlikeCommentCommand : IUnlikeCommentCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;
        private readonly ILikeService _likeService;

        public EFUnlikeCommentCommand(BlogContext context, IApplicationActor actor, ILikeService likeService)
        {
            _context = context;
            _actor = actor;
            _likeService = likeService;
        }

        public int Id => (int)UseCaseEnum.EFUnlikeCommentCommand;
        public string Name => UseCaseEnum.EFUnlikeCommentCommand.ToString();

        public async Task ExecuteAsync(LikeDto request)
        {
            if (request.IdComment == null)
            {
                throw new ArgumentException("IdComment is required.");
            }

            var comment = await _context.Comments.FindAsync(request.IdComment);
            if (comment == null)
            {
                throw new EntityNotFoundException((int)request.IdComment, typeof(Domain.Comment));
            }

            await _likeService.RemoveLike(request);

            await _context.SaveChangesAsync();
        }
    }
}