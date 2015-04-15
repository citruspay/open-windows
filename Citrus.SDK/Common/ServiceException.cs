using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Common
{
    public class ServiceException : ApplicationException
    {
        public ServiceException()
        {

        }

        public ServiceException(string message)
            : base(message)
        {

        }
    }
}
