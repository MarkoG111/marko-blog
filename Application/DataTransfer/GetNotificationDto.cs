using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer
{
    public class GetNotificationDto
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public NotificationType Type { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public string Link { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}