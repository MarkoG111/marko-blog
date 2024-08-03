using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class UserLikeException : Exception
    {
        public UserLikeException(IApplicationActor actor)
            : base("You can't like or dislike your own comment.")
        {

        }
    }
}