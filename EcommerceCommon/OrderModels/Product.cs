using System;
using System.Collections.Generic;

#nullable disable

namespace EcommerceCommon.OrderModels
{
    public partial class Product
    {
        public Product()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        public int Productid { get; set; }
        public string Productname { get; set; }
        public decimal? Packheight { get; set; }
        public decimal? Packwidth { get; set; }
        public decimal? Packweight { get; set; }
        public string Colour { get; set; }
        public string Size { get; set; }

        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
