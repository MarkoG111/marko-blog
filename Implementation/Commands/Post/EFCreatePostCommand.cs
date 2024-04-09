using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application;
using Application.Commands.Post;
using Application.DataTransfer;
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

        public EFCreatePostCommand(CreatePostValidator validator, IApplicationActor actor, BlogContext context)
        {
            _validator = validator;
            _actor = actor;
            _context = context;
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

            _context.Posts.Add(post);
            _context.SaveChanges();

            request.Id = post.Id;
        }
    }
}