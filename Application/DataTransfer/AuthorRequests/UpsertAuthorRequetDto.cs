using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer.AuthorRequests
{
    public class UpsertAuthorRequestDto
    {
        public int Id { get; set; }
        public RequestStatus Status { get; set; }
        public string Reason { get; set; }
        public int IdUser { get; set; }
        public int IdRole { get; set; }
    }
}