using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; } = new HashSet<Blog>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Like> Likes { get; set; } = new HashSet<Like>();
        public virtual ICollection<UserUseCase> UserUseCases { get; set; } = new HashSet<UserUseCase>();
    }
}