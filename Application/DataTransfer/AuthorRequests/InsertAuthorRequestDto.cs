using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer.AuthorRequests
{
    public class InsertAuthorRequestDto
    {
        public int IdUser { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime DateCreated { get; set; }
        public string Reason { get; set; }
        public RequestStatus Status { get; set; }
    }
}