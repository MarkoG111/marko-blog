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

        GetPostDto IQuery<GetPostDto, int>.Execute(int search)
        {
            var post = _context.Posts.Find(search);

            var query = _context.Posts.Include(l => l.Likes)
                .Include(i => i.Image)
                .Include(com => com.Comments)
                .Include(u => u.User)
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
                Username = query.User.Username,
                Categories = query.PostCategories.Select(x => new CategoryDto
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                }).ToList(),

                Likes = query.Likes.Select(z => new LikePostDto
                {
                    Id = z.Id,
                    Status = z.Status,
                    Username = z.User.Username,
                }).ToList(),

                Comments = query.Comments.Select(t => new SingleCommentDto
                {
                    Id = t.Id,
                    Text = t.CommentText,
                    CreatedAt = t.CreatedAt,
                    Username = t.User.Username,
                    IdUser = t.User.Id,
                    Children = t.ChildrenComments.Select(c => new SingleCommentDto
                    {
                        Id = c.Id,
                        IdParent = c.IdParent.Value,
                        Text = c.CommentText,
                        CreatedAt = c.CreatedAt,
                        IdUser = t.User.Id,
                        Username = t.User.Username
                    }).ToList()
                }).ToList()
            };

            foreach (var res in result.Comments)
            {
                if (res.IdParent == null)
                {
                    parentComments.Add(res);
                }
            }

            result.Comments = parentComments;

            return result;
        }
    }
}