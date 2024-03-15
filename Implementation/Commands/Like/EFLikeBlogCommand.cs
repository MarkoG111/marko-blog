using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Like;
using Application.DataTransfer;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Like;

namespace Implementation.Commands.Like
{
    public class EFLikeBlogCommand : ILikeBlogCommand
    {
        private readonly BlogContext _context;
        private readonly LikeValidator _validator;

        public EFLikeBlogCommand(LikeValidator validator, BlogContext context)
        {
            _validator = validator;
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFLikeBlog;
        public string Name => UseCaseEnum.EFLikeBlog.ToString();

        public void Execute(LikeDto request)
        {
            _validator.ValidateAndThrow(request);

            var findLike = _context.Likes.Where(x => x.IdBlog == request.IdBlog && x.IdUser == request.IdUser).FirstOrDefault();

            if (findLike == null)
            {
                var like = new Domain.Like
                {
                    IdUser = request.IdUser,
                    IdBlog = request.IdBlog,
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
        }

    }
}