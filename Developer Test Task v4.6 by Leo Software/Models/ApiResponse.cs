using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Developer_Test_Task_v4._6_by_Leo_Software.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}