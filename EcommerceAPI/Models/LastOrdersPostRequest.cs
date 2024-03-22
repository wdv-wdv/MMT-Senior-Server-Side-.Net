using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceAPI.Models
{
    public class LastOrdersPostRequest
    {
        public string User { get; set; }
        public string CustomerID { get; set; }
    }
}
