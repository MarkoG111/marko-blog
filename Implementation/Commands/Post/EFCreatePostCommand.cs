using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Application.Commands.Post;
using Application.DataTransfer;
using Application.Services;
using Domain;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Post;

namespace Implementation.Commands.Post
{
    public class EFCreatePostCommand : ICreatePostCommand
    {
        private readonly BlogContext _context;
        private readonly IApplicationActor _actor;
        private readonly CreatePostValidator _validator;
        private readonly INotificationHubService _notificationHubService;
        private readonly INotificationService _notificationService;

        public EFCreatePostCommand(CreatePostValidator validator, IApplicationActor actor, BlogContext context, INotificationHubService notificationHubService, INotificationService notificationService)
        {
            _validator = validator;
            _actor = actor;
            _context = context;
            _notificationHubService = notificationHubService;
            _notificationService = notificationService;
        }

        public int Id => (int)UseCaseEnum.EFCreatePostCommand;
        public string Name => UseCaseEnum.EFCreatePostCommand.ToString();

        public void Execute(InsertPostDto request)
        {
            _validator.ValidateAndThrow(request);

            var post = new Domain.Post
            {
                Title = request.Title,
                Content = request.Content,
                IdImage = request.IdImage,
                IdUser = _actor.Id
            };

            foreach (var category in request.PostCategories)
            {
                post.PostCategories.Add(new PostCategory
                {
                    IdCategory = category.IdCategory
                });
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Posts.Add(post);
                    _context.SaveChanges();

                    request.Id = post.Id;

                    var followers = _context.Followers.Where(f => f.IdFollowing == post.IdUser).Select(f => f.IdFollower).ToList();

                    foreach (var idFollower in followers)
                    {
                        var notificationDto = new NotificationDto
                        {
                            IdUser = idFollower,
                            FromIdUser = post.IdUser,
                            Type = NotificationType.Post,
                            Content = $"{_actor.Identity} has published a new post: {post.Title}",
                            CreatedAt = DateTime.Now,
                            IdPost = post.Id
                        };

                        _notificationService.CreateNotification(notificationDto);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}