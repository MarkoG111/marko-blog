using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Services;
using EFDataAccess;
using Domain;
using FluentValidation;
using Implementation.Validators.Comment;

namespace Implementation.Commands.Comment
{
    public class EFCreateCommentCommand : ICreateCommentCommand
    {
        private readonly BlogContext _context;
        private readonly CreateCommentValidator _validator;
        private readonly IApplicationActor _actor;
        private readonly INotificationHubService _notificationHubService;
        private readonly INotificationService _notificationService;

        public EFCreateCommentCommand(BlogContext context, CreateCommentValidator validator, IApplicationActor actor, INotificationHubService notificationHubService, INotificationService notificationService)
        {
            _context = context;
            _validator = validator;
            _actor = actor;
            _notificationHubService = notificationHubService;
            _notificationService = notificationService;
        }

        public int Id => (int)UseCaseEnum.EFCreateCommentCommand;

        public string Name => UseCaseEnum.EFCreateCommentCommand.ToString();

        public void Execute(InsertCommentDto request)
        {
            _validator.ValidateAndThrow(request);

            request.IdUser = _actor.Id;

            var comment = new Domain.Comment
            {
                CommentText = request.CommentText,
                IdPost = request.IdPost,
                IdParent = request.IdParent,
                IdUser = request.IdUser
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            request.Id = comment.Id;

            var post = _context.Posts.Where(x => x.Id == request.IdPost).Select(x => new { x.IdUser, x.Title }).FirstOrDefault();

            if (post == null)
            {
                throw new Exception("Post not found");
            }

            // Avoid sending a notification to the commenter themselves
            if (post.IdUser != request.IdUser)
            {
                var postOwnerNotification = new NotificationDto
                {
                    IdUser = post.IdUser, // Post owner
                    FromIdUser = request.IdUser, // Commenter
                    Type = NotificationType.Comment,
                    Content = $"{_actor.Identity} has commented on your post.",
                    CreatedAt = DateTime.Now,
                    IdComment = comment.Id
                };

                _notificationService.CreateNotification(postOwnerNotification);
            }

            // If the comment is a reply, notify the parent comment's owner (if different from the commenter)
            if (request.IdParent.HasValue)
            {
                var parentComment = _context.Comments
                    .Where(c => c.Id == request.IdParent.Value)
                    .Select(c => new { c.IdUser, c.CommentText })
                    .FirstOrDefault();

                if (parentComment != null && parentComment.IdUser != request.IdUser)
                {
                    var parentCommentNotification = new NotificationDto
                    {
                        IdUser = parentComment.IdUser, // Parent comment owner
                        FromIdUser = request.IdUser, // Commenter
                        Type = NotificationType.Comment,
                        Content = $"{_actor.Identity} has replied to your comment: \"{parentComment.CommentText}\"",
                        CreatedAt = DateTime.Now,
                        IdComment = comment.Id
                    };

                    _notificationService.CreateNotification(parentCommentNotification);
                }
            }
        }
    }
}