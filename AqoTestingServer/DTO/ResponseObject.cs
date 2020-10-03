using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AqoTestingServer.DTO
{
    public class ResponseObject
    {
        public bool Success { get; set; }
        public dynamic Data { get; set; }
    }
}
