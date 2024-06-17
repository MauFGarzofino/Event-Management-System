using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventManagementSystemAPI.Models
{
    public class Response<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }

        public T Data { get; set; }
        
        public Response(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Errors = null;
            Data = data;
                       
        }

    }
}
