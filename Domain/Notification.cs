using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Notification : BaseEntity
    {
        public int IdUser { get; set; }
        public int FromIdUser { get; set; }
        public virtual User UserReceiver { get; set; }
        public virtual User FromUser { get; set; }
        public NotificationType Type { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public bool IsRead { get; set; }
    }

    public enum NotificationType
    {
        Post = 1,
        Comment = 2,
        Like = 3,
    }
}