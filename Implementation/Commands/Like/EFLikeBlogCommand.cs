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
    public class EFLikePostCommand : ILikePostCommand
    {
        private readonly BlogContext _context;
        private readonly LikeValidator _validator;

        public EFLikePostCommand(LikeValidator validator, BlogContext context)
        {
            _validator = validator;
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFLikePost;
        public string Name => UseCaseEnum.EFLikePost.ToString();

        public void Execute(LikeDto request)
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
        }

    }
}