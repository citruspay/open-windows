namespace Citrus.SDK.Common
{
    using System;

    public class ServiceException : Exception
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
