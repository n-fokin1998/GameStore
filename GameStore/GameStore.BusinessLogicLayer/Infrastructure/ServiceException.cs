using System;

namespace GameStore.BusinessLogicLayer.Infrastructure
{
    public class ServiceException : Exception
    {
        public ServiceException(string message, string property) : base(message)
        {
            Property = property;
        }

        public string Property { get; protected set; }
    }
}