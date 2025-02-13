using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Exceptions;
using Application.DataTransfer.Likes;
using Application.DataTransfer.Notifications;
using Application.Commands.Like;
using Application.Services;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Like;
using Implementation.Services;
using Domain;

namespace Implementation.Commands.Like
{
    public class EFLikeCommentCommand : ILikeCommentCommand
    {
        private readonly BlogContext _context;
        private readonly LikeCommentValidator _validator;
        private readonly IApplicationActor _actor;
        private readonly ILikeService _likeService; 
        private readonly INotificationHubService _notificationHubService;
        private readonly INotificationService _notificationService;

        public EFLikeCommentCommand(LikeCommentValidator validator, BlogContext context, IApplicationActor actor, ILikeService likeService, INotificationHubService notificationHubService, INotificationService notificationService)
        {
            _validator = validator;
            _context = context;
            _actor = actor;
            _likeService = likeService;
            _notificationHubService = notificationHubService;
            _notificationService = notificationService;
        }

        public int Id => (int)UseCaseEnum.EFLikeComment;
        public string Name => UseCaseEnum.EFLikeComment.ToString();

        public async Task ExecuteAsync(LikeDto request)
        {
            _validator.ValidateAndThrow(request);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var like = _likeService.ToggleLike(request);

                    if (like != null)
                    {
                        var comment = _context.Comments.FirstOrDefault(x => x.Id == request.IdComment);

                        await _notificationService.CreateNotification(new InsertNotificationDto
                        {
                            IdUser = comment.IdUser,
                            FromIdUser = _actor.Id,
                            Type = NotificationType.Like,
                            Content = $"{_actor.Identity} liked your comment.",
                            IdPost = request.IdPost,
                            IdComment = request.IdComment,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await transaction.CommitAsync();
                }
                catch (System.Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}