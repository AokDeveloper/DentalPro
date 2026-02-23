using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalApp.Domain.DomainExceptionHandler
{
    public class InvalidActionException : DomainException
    {
        public InvalidActionException(string message) : base(message) { }
    }
}
