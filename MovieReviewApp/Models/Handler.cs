using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.Models
{
    public class Handler
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
