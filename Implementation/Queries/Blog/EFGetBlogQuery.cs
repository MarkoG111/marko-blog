using Application;
using Application.DataTransfer;
using Application.Queries.Blog;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Implementation.Queries.Blog
{
    public class EFGetBlogQuery : IGetBlogQuery
    {
        private readonly BlogContext _context;

        public EFGetBlogQuery(BlogContext context)
        {
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFGetOneBlogQuery;
        public string Name => UseCaseEnum.EFGetOneBlogQuery.ToString();

        public ICollection<SingleCommentDto> parentComments { get; set; } = new List<SingleCommentDto>();

        GetBlogDto IQuery<GetBlogDto, int>.Execute(int search)
        {
            var blog = _context.Blogs.Find(search);

            var query = _context.Blogs.Include(l => l.Likes)
                .Include(i => i.Image)
                .Include(com => com.Comments)
                .Include(u => u.User)
                .Include(bc => bc.BlogCategories)
                .ThenInclude(c => c.Category)
                .FirstOrDefault(a => a.Id == search);

            var result = new GetBlogDto
            {
                Id = query.Id,
                Title = query.Title,
                Content = query.Content,
                DateCreated = query.CreatedAt,
                ImageName = query.Image.ImagePath,
                IdImage = query.IdImage,
                Username = query.User.Username,
                Categories = query.BlogCategories.Select(x => new CategoryDto
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                }).ToList(),

                Likes = query.Likes.Select(z => new LikeBlogDto
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
                    Children = t.ChildrenComments.Select(c => new SingleCommentDto
                    {
                        Id = c.Id,
                        IdParent = c.IdParent.Value,
                        Text = c.CommentText,
                        CreatedAt = c.CreatedAt,
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