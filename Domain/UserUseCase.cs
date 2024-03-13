using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class UserUseCase : BaseEntity
    {
        public int IdUser { get; set; }
        public int IdUseCase { get; set; }
        public virtual User User { get; set; }
    }
}