using MaaCopilot.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Service
{
    public class ServiceResponse<T> : IServiceResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T ResponseObject { get; set; }
        public ServiceResponse(bool success, string message) : this(success, message, default(T))
        {

        }
        public ServiceResponse(bool success, string message, T responseObject)
        {
            Success = success;
            Message = message;
            ResponseObject = responseObject;
        }

        public ServiceResponse()
        {
        }
    }
}
