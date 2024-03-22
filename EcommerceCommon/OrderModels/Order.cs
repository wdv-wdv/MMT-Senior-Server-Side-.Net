using System;
using System.Collections.Generic;

#nullable disable

namespace EcommerceCommon.OrderModels
{
    public partial class Order
    {
        public Order()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        public int Orderid { get; set; }
        public string Customerid { get; set; }
        public DateTime? Orderdate { get; set; }
        public DateTime? Deliveryexpected { get; set; }
        public bool? Containsgift { get; set; }
        public string Shippingmode { get; set; }
        public string Ordersource { get; set; }

        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
