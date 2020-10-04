using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message) :base(message)
        {

        }
    }
}
