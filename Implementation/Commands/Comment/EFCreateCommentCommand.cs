using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Comment;
using Application.DataTransfer;
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
        private readonly INotificationHubService _notificationService;

        public EFCreateCommentCommand(BlogContext context, CreateCommentValidator validator, IApplicationActor actor, INotificationHubService notificationService)
        {
            _context = context;
            _validator = validator;
            _actor = actor;
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

            // Create a list to hold notifications (e.g., for post owner and parent comment user)
            var notifications = new List<Domain.Notification>();

            // Avoid sending a notification to the commenter themselves
            if (post.IdUser != request.IdUser)
            {
                notifications.Add(new Domain.Notification
                {
                    IdUser = post.IdUser, // Post owner
                    FromIdUser = request.IdUser, // Commenter
                    Type = NotificationType.Comment,
                    Content = $"{_actor.Identity} has commented on your post.",
                    IsRead = false
                });
            }

            if (request.IdParent.HasValue)
            {
                var parentComment = _context.Comments
                    .Where(c => c.Id == request.IdParent.Value)
                    .Select(c => new { c.IdUser, c.CommentText })
                    .FirstOrDefault();

                if (parentComment != null && parentComment.IdUser != request.IdUser)
                {
                    notifications.Add(new Domain.Notification
                    {
                        IdUser = parentComment.IdUser, // Parent comment owner
                        FromIdUser = request.IdUser, // Commenter
                        Type = NotificationType.Comment,
                        Content = $"{_actor.Identity} has replied to your comment: \"{parentComment.CommentText}\"",
                        IsRead = false
                    });
                }
            }

            _context.Notifications.AddRange(notifications);
            _context.SaveChanges();

            foreach (var notification in notifications)
            {
                _notificationService.SendNotificationToUser(notification.IdUser, notification);
            }

        }
    }
}