using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.DataTransfer.Users;
using Application.DataTransfer.Posts;
using Application.DataTransfer.UseCases;
using Application.DataTransfer.Likes;
using Application.DataTransfer.Comments;
using Application.Queries.User;
using EFDataAccess;

namespace Implementation.Queries.User
{
    public class EFGetOneUserQuery : IGetUserQuery
    {
        private readonly BlogContext _context;

        public EFGetOneUserQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneUserQuery;
        public string Name => UseCaseEnum.EFGetOneUserQuery.ToString();

        public GetUserDto Execute(int idUser)
        {
            var user = _context.Users
            .Include(x => x.UserUseCases)
            .Include(u => u.Role)
            .Include(p => p.Posts.OrderByDescending(p => p.CreatedAt))
                .ThenInclude(bc => bc.PostCategories)
                .ThenInclude(cat => cat.Category)
            .Include(p => p.Posts)
                .ThenInclude(i => i.Image) // Separate include for Image
            .Include(c => c.Comments)
                .ThenInclude(pos => pos.Post)
                .ThenInclude(i => i.Image)
            .Include(l => l.Likes)
            .FirstOrDefault(x => x.Id == idUser);

            if (user == null)
            {
                return null;
            }

            var followersCount = _context.Followers.Count(f => f.IdFollowing == idUser);
            var followingCount = _context.Followers.Count(f => f.IdFollower == idUser);
            var postsCount = _context.Posts.Count(p => p.IdUser == idUser);

            return new GetUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                ProfilePicture = user.ProfilePicture,
                RoleName = user.Role.Name,
                UserUseCases = user.UserUseCases.Select(x => new GetUserUseCaseDto
                {
                    IdUseCase = x.IdUseCase
                }),
                UserPosts = user.Posts.Select(p => new GetUserPostsDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImageName = p.Image.ImagePath,
                    DateCreated = p.CreatedAt,
                    IdUser = p.IdUser,
                    Categories = p.PostCategories.Select(y => new GetPostCategoriesDto
                    {
                        Id = y.Category.Id,
                        Name = y.Category.Name
                    }).ToList()
                }).ToList(),
                UserComments = user.Comments.Select(c => new GetUserCommentsDto
                {
                    Id = c.Id,
                    CommentText = c.CommentText,
                    PostTitle = c.Post.Title,
                    CreatedAt = c.CreatedAt
                }).ToList(),
                CommentLikes = user.Likes.Select(l => new LikeDto
                {
                    IdUser = l.IdUser,
                    IdPost = l.IdPost,
                    IdComment = l.IdComment,
                    Status = l.Status
                }).ToList(),
                FollowersCount = followersCount,
                FollowingCount = followingCount,
                PostsCount = postsCount
            };
        }
    }
}