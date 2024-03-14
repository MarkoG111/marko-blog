using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(int id, Type type)
            : base($"Entity of type {type.Name} with an ID of {id} was not found.")
        {

        }
    }
}