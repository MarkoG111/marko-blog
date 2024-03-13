using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Commands.Category;
using Application.DataTransfer;
using Domain;
using EFDataAccess;
using FluentValidation;
using Implementation.Validators.Category;

namespace Implementation.Commands.Category
{
    public class EFCreateCategoryCommand : ICreateCategoryCommand
    {
        private readonly BlogContext _context;
        private readonly CreateCategoryValidator _validator;

        public EFCreateCategoryCommand(CreateCategoryValidator validator, BlogContext context)
        {
            _validator = validator;
            _context = context;
        }

        public int Id => (int)UseCaseEnum.EFCreateCategoryCommand;
        public string Name => UseCaseEnum.EFCreateCategoryCommand.ToString();

        public void Execute(CategoryDto request)
        {
            _validator.ValidateAndThrow(request);

            var category = new Domain.Category
            {
                Name = request.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
        }
    }
}