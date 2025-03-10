using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace Application.DataTransfer.Notifications
{
    public class InsertNotificationDto
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdPost { get; set; }
        public int? IdComment { get; set; }
        public int FromIdUser { get; set; }
        public NotificationType Type { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}