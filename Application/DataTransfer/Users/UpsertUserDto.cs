using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DataTransfer.UseCases;
using Microsoft.AspNetCore.Http;

namespace Application.DataTransfer.Users
{
    public class UpsertUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile? Image { get; set; }
        public int IdRole { get; set; }
        public IEnumerable<GetUserUseCaseDto> UserUseCases { get; set; }
    }
}