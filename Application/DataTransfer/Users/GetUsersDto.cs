using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DataTransfer.Users
{
    public class GetUsersDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}