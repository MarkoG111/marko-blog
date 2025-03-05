using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Post;
using Application.DataTransfer.Posts;
using Application.Exceptions;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Post;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Commands.Post
{
    public class EFUpdatePostCommand : IUpdatePostCommand
    {
        private readonly BlogContext _context;
        private readonly UpdatePostValidator _validator;

        public EFUpdatePostCommand(BlogContext context, UpdatePostValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        public int Id => (int)UseCaseEnum.EFUpdatePostCommand;
        public string Name => UseCaseEnum.EFUpdatePostCommand.ToString();

        public async Task ExecuteAsync(UpsertPostDto request)
        {
            _validator.ValidateAndThrow(request);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var post = await _context.Posts
                        .Include(x => x.PostCategories)
                        .FirstOrDefaultAsync(x => x.Id == request.Id);

                    if (post == null)
                    {
                        throw new EntityNotFoundException(request.Id, typeof(Domain.Post));
                    }

                    post.Title = request.Title;
                    post.Content = request.Content;
                    post.IdImage = request.IdImage;
                    post.ModifiedAt = DateTime.UtcNow;

                    // Get current category IDs associated with the post
                    var currentCategoryIds = post.PostCategories.Select(pc => pc.IdCategory).ToList();

                    // Find category IDs to remove (existing IDs not in the request)
                    var categoryIdsToRemove = currentCategoryIds.Except(request.CategoryIds).ToList();

                    // Find category IDs to add (IDs in the request but not currently associated)
                    var categoryIdsToAdd = request.CategoryIds.Except(currentCategoryIds).ToList();

                    // Remove categories
                    foreach (var categoryId in categoryIdsToRemove)
                    {
                        var postCategory = post.PostCategories.First(pc => pc.IdCategory == categoryId);
                        _context.PostCategories.Remove(postCategory);
                    }

                    // Add new categories
                    foreach (var categoryId in categoryIdsToAdd)
                    {
                        post.PostCategories.Add(new Domain.PostCategory
                        {
                            IdPost = post.Id,
                            IdCategory = categoryId,
                            IsActive = true
                        });
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
