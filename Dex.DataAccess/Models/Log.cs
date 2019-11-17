using System;
using System.Collections.Generic;

namespace Dex.DataAccess.Models
{
    public partial class Log
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
    }
}
