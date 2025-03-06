using Application;
using Application.DataTransfer.Posts;
using Application.DataTransfer.Comments;
using Application.Queries.Post;
using Application.Exceptions;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Queries.Post
{
    public class EFGetOnePostQuery : IGetPostQuery
    {
        private readonly BlogContext _context;

        public EFGetOnePostQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOnePostQuery;
        public string Name => UseCaseEnum.EFGetOnePostQuery.ToString();

        GetPostDetailsDto IQuery<GetPostDetailsDto, int>.Execute(int idPost)
        {
            var query = _context.Posts
                .Include(l => l.Likes)
                .Include(up => up.User)
                .Include(i => i.Image)
                .Include(com => com.Comments)
                    .ThenInclude(u => u.User)
                .Include(com => com.Comments)
                    .ThenInclude(l => l.Likes)
                .Include(bc => bc.PostCategories)
                    .ThenInclude(c => c.Category)
                .FirstOrDefault(a => a.Id == idPost);

            if (query == null)
            {
                throw new EntityNotFoundException(idPost, typeof(Domain.Post));
            }

            var comments = (query.Comments ?? new List<Domain.Comment>()).ToList();

            var commentsLookup = comments.GroupBy(c => c.IdParent ?? 0).ToDictionary(g => g.Key, g => g.ToList());

            List<GetCommentsDto> BuildCommentTree(int? idParent)
            {
                return commentsLookup.ContainsKey(idParent ?? 0) ? commentsLookup[idParent ?? 0].Select(c => new GetCommentsDto
                {
                    Id = c.Id,
                    CommentText = c.CommentText,
                    IdParent = c.IdParent,
                    IdUser = c.IdUser,
                    Username = c.User?.Username,
                    CreatedAt = c.CreatedAt,
                    IsDeleted = c.IsDeleted,
                    LikesCount = c.Likes.Count,
                    Likes = c.Likes.Select(l => new GetCommentLikesDto
                    {
                        IdComment = l.IdComment,
                        IdUser = l.IdUser,
                        Status = l.Status,
                    }).ToList(),
                    ChildrenComments = BuildCommentTree(c.Id)
                }).ToList() : new List<GetCommentsDto>();
            }

            var result = new GetPostDetailsDto
            {
                Id = query.Id,
                Title = query.Title,
                Content = query.Content,
                DateCreated = query.CreatedAt,
                ImageName = query.Image.ImagePath,
                IdImage = query.Image.Id,
                IdUser = query.User.Id,
                Username = query.User?.Username,
                Categories = query.PostCategories.Select(x => new GetPostCategoriesDto
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                }).ToList(),
                Likes = query.Likes.Select(w => new GetPostLikesDto
                {
                    IdPost = w.Post.Id,
                    Status = w.Status,
                    IdUser = w.IdUser
                }).ToList(),
                Comments = BuildCommentTree(null)
            };

            return result;
        }
    }
}