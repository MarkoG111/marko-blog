using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class AlreadyDeletedException : Exception
    {
        public AlreadyDeletedException(int id, Type type)
            : base($"Entity of type {type.Name} with and ID of {id} was already deleted")
        {

        }
    }
}