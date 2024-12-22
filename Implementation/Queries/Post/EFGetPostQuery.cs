using Application;
using Application.DataTransfer.Posts;
using Application.DataTransfer.Comments;
using Application.Queries.Post;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Queries.Post
{
    public class EFGetPostQuery : IGetPostQuery
    {
        private readonly BlogContext _context;

        public EFGetPostQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOnePostQuery;
        public string Name => UseCaseEnum.EFGetOnePostQuery.ToString();

        GetPostDetailsDto IQuery<GetPostDetailsDto, int>.Execute(int search)
        {
            var post = _context.Posts.Find(search);

            var query = _context.Posts
                .Include(l => l.Likes)
                .Include(up => up.User)
                .Include(i => i.Image)
                .Include(com => com.Comments)
                .ThenInclude(u => u.User)
                .Include(bc => bc.PostCategories)
                .ThenInclude(c => c.Category)
                .FirstOrDefault(a => a.Id == search);

            var result = new GetPostDetailsDto
            {
                Id = query.Id,
                Title = query.Title,
                Content = query.Content,
                DateCreated = query.CreatedAt,
                ImageName = query.Image.ImagePath,
                Username = query.User?.Username,
                Categories = query.PostCategories.Select(x => new GetPostCategoriesDto
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                }).ToList(),
                Likes = query.Likes.Select(w => new GetPostLikesDto
                {
                    Id = w.Id,
                    Status = w.Status,
                    IdUser = w.IdUser
                }).ToList(),
                Comments = query.Comments.Where(c => c.IdParent == null).Select(t => new GetCommentsDto
                {
                    Id = t.Id,
                    CommentText = t.CommentText,
                    IdParent = t.IdParent,
                    IdUser = t.IdUser,
                    Username = t.User?.Username,
                    CreatedAt = t.CreatedAt,
                    IsDeleted = t.IsDeleted,
                    Likes = t.Likes.Select(l => new GetCommentLikesDto
                    {
                        IdComment = l.IdComment,
                        IdUser = l.IdUser,
                        Status = l.Status,
                    }).ToList(),
                    ChildrenComments = query.Comments.Where(c => c.IdParent == t.Id).Select(c => new GetCommentsDto
                    {
                        Id = c.Id,
                        CommentText = c.CommentText,
                        IdParent = c.IdParent,
                        IdUser = c.IdUser,
                        Username = c.User?.Username,
                        CreatedAt = c.CreatedAt,
                        IsDeleted = c.IsDeleted,
                        Likes = c.Likes.Select(l => new GetCommentLikesDto
                        {
                            IdComment = l.IdComment,
                            IdUser = l.IdUser,
                            Status = l.Status,
                        }).ToList(),
                    }).ToList(),
                }).ToList(),
            };

            return result;
        }
    }
}