using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.DTOS
{
    public class APIResponse<T>
    {
        public APIResponse(T data, bool success = true, string message = "")
        {
            Data = data;
            Success = success;
            errorMessage = message;
        }
        public APIResponse()
        {

        }
        public string errorMessage { get; set; }
        public bool Success { get; set; } = true;
        public T Data { get; set; }
    }
}

