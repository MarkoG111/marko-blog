using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer
{
    public class UpdateAuthorRequestDto
    {
        public int Id { get; set; }
        public RequestStatus Status { get; set; }
        public int IdRole { get; set; }
    }
}