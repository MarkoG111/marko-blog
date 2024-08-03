using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class AuthorRequest : BaseEntity
    {
        [ForeignKey("User")]
        [Column("IdUser")]
        public int IdUser { get; set; }
        public string Reason { get; set; }
        public RequestStatus Status { get; set; }
        public virtual User User { get; set; }
    }

    public enum RequestStatus
    {
        Pending = 1,
        Accepted = 2,
        Rejected = 3
    }
}