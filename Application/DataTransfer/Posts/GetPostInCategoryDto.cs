using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Posts
{
  public class GetPostInCategoryDto
  {
      public int Id { get; set; }
      public string Title { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public DateTime DateCreated { get; set; }
      public string ProfilePicture { get; set; }
      public IEnumerable<GetPostCategoriesDto> Categories { get; set; } = new List<GetPostCategoriesDto>();
  }
}