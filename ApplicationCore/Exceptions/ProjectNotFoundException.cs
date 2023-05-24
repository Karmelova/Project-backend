using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string? message) : base(message)
        {
        }
    }
}