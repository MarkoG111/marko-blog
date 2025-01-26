using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.Followers;
using EFDataAccess;
using FluentValidation;

namespace Implementation.Validators.Follow
{
    public class FollowUserValidator : AbstractValidator<InsertFollowDto>
    {
        private readonly BlogContext _context;

        public FollowUserValidator(BlogContext context)
        {
            _context = context;

            RuleFor(x => x.IdUser)
               .NotEmpty()
               .WithMessage("User ID is required.")
               .Must(UserExists)
               .WithMessage("User with the provided ID does not exist.");

            RuleFor(x => x.IdFollowing)
                .NotEmpty()
                .WithMessage("Following ID is required.")
                .Must(UserExists)
                .WithMessage("User to follow does not exist.")
                .Must((dto, idFollowing) => idFollowing != dto.IdUser)
                .WithMessage("A user cannot follow themselves.");

            RuleFor(x => x.FollowedAt)
                .NotEmpty()
                .WithMessage("FollowedAt date is required.")
                .Must(date => date <= DateTime.UtcNow)
                .WithMessage("FollowedAt date cannot be in the future.");

            RuleFor(x => x)
                .Must(dto => !IsAlreadyFollowing(dto.IdUser, dto.IdFollowing))
                .WithMessage("The user is already following the specified user.");
        }

        private bool UserExists(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        private bool IsAlreadyFollowing(int idUser, int idFollowing)
        {
            return _context.Followers.Any(f => f.IdFollower == idUser && f.IdFollowing == idFollowing);
        }
    }
}