using Application;
using Application.DataTransfer;
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

        public ICollection<SingleCommentDto> parentComments { get; set; } = new List<SingleCommentDto>();
        public ICollection<SingleCommentDto> childrenComments { get; set; } = new List<SingleCommentDto>();

        GetPostDto IQuery<GetPostDto, int>.Execute(int search)
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

            var result = new GetPostDto
            {
                Id = query.Id,
                Title = query.Title,
                Content = query.Content,
                DateCreated = query.CreatedAt,
                ImageName = query.Image.ImagePath,
                IdImage = query.IdImage,
                Username = query.User?.Username,
                Categories = query.PostCategories.Select(x => new CategoryDto
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                }).ToList(),
                Likes = query.Likes.Select(w => new GetLikePostDto
                {
                    Id = w.Id,
                    Status = w.Status,
                    IdUser = w.IdUser
                }).ToList(),
                Comments = query.Comments.Select(t => new SingleCommentDto
                {
                    Id = t.Id,
                    CommentText = t.CommentText,
                    CreatedAt = t.CreatedAt,
                    IdUser = t.IdUser,
                    IdParent = t.IdParent,
                    Username = t.User?.Username,
                    IsDeleted = t.IsDeleted,
                    LikesCount = t.Likes.Count(l => l.IdComment != null),
                    Likes = t.Likes.Select(l => new LikeCommentDto
                    {
                        IdComment = l.IdComment,
                        IdUser = l.IdUser,
                        Status = l.Status,
                    }).ToList(),
                    Children = t.ChildrenComments.Select(c => new SingleCommentDto
                    {
                        Id = c.Id,
                        IdParent = c.IdParent,
                        CommentText = c.CommentText,
                        CreatedAt = c.CreatedAt,
                        IdUser = t.IdUser,
                        Username = c.User?.Username,
                        IsDeleted = c.IsDeleted
                    }).ToList()
                }).ToList()
            };

            foreach (var res in result.Comments)
            {
                if (res.IdParent == null)
                {
                    parentComments.Add(res);
                }
                else
                {
                    childrenComments.Add(res);
                }
            }

            result.Comments = parentComments;
            result.ChildrenComments = childrenComments;

            return result;
        }
    }
}