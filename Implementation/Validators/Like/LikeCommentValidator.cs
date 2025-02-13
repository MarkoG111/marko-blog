using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Likes;
using Domain;
using FluentValidation;

namespace Implementation.Validators.Like
{
    public class LikeCommentValidator : AbstractValidator<LikeDto>
    {
        public LikeCommentValidator()
        {
            RuleFor(x => x.IdPost).NotEmpty();
            RuleFor(x => x.IdUser).NotEmpty();
            RuleFor(x => x.IdComment).NotEmpty();
            RuleFor(x => x.Status).NotEmpty()
                .Must(y => Enum.IsDefined(typeof(LikeStatus), y))
                .WithMessage("Status can only be 'Like' or 'Dislike'.");
        }
    }
}