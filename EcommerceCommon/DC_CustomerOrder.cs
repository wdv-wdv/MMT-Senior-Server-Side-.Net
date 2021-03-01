using System;
using System.Collections.Generic;

namespace EcommerceCommon
{
    public class DC_CustomerOrder
	{
        public DC_Customer Customer { get; set; }
        public DC_Order Order { get; set; }
        public class DC_Customer
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        public class DC_Order
        {
            public class DC_OrderItem
            {
                public string Product { get; set; }
                public int Quantity { get; set; }
                public Decimal? PriceEach { get; set; }
            }

            public int OrderNumber { get; set; }
            public DateTime OrderDate { get; set; }
            public string DeliveryAddress { get; set; }
            public List<DC_OrderItem> OrderItems { get; set; }
            public DateTime DeliveryExpected { get; set; }
        }
    }
}
