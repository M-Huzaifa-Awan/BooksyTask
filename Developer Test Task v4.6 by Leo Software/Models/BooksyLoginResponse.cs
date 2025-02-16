using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Developer_Test_Task_v4._6_by_Leo_Software.Models
{
    public class BooksyLoginResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}