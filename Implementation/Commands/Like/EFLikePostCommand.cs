using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Exceptions;
using Application.Commands.Like;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Like;
using Domain;

namespace Implementation.Commands.Like
{
    public class EFLikePostCommand : ILikePostCommand
    {
        private readonly BlogContext _context;
        private readonly LikePostValidator _validator;
        private readonly IApplicationActor _actor;
        private readonly INotificationHubService _notificationService;

        public EFLikePostCommand(LikePostValidator validator, BlogContext context, IApplicationActor actor, INotificationHubService notificationService)
        {
            _validator = validator;
            _context = context;
            _actor = actor;
            _notificationService = notificationService;
        }

        public int Id => (int)UseCaseEnum.EFLikePost;
        public string Name => UseCaseEnum.EFLikePost.ToString();

        public void Execute(LikePostDto request)
        {
            _validator.ValidateAndThrow(request);

            var findLike = _context.Likes.Where(x => x.IdPost == request.IdPost && x.IdUser == request.IdUser).FirstOrDefault();

            if (findLike == null)
            {
                var like = new Domain.Like
                {
                    IdUser = request.IdUser,
                    IdPost = request.IdPost,
                    Status = request.Status
                };

                _context.Likes.Add(like);
                _context.SaveChanges();
            }
            else
            {
                findLike.Status = request.Status;
                _context.SaveChanges();
            }

            var post = _context.Posts.Find(request.IdPost);

            if (post == null)
            {
                throw new EntityNotFoundException(request.IdPost, typeof(Domain.Post));
            }

            var notification = new Domain.Notification
            {
                IdUser = post.IdUser,
                FromIdUser = _actor.Id,
                Type = NotificationType.Like,
                Content = $"{_actor.Identity} liked your post.",
                IsRead = false
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            _notificationService.SendNotificationToUser(post.IdUser, $"{_actor.Identity} liked your post.");
        }

    }
}