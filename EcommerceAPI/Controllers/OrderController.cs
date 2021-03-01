using EcommerceCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("v1/orders")]
    public class OrderController : ControllerBase
    {

        [HttpPost("LastOrders")]
        public object LastOrders([FromBody]DC_LastOrdersPostRequest lastOrdersPostRequest)
        {
            DC_CustomerDetails customerDetails = Datalayer.GetCustomer(lastOrdersPostRequest.User);

            //### check customerIDs match ###
            if (customerDetails is DC_CustomerDetails && customerDetails.customerId.Equals(lastOrdersPostRequest.CustomerID, StringComparison.OrdinalIgnoreCase))
            {
                //### create payload object ###
                DC_CustomerOrder customerOrder = new DC_CustomerOrder();
                customerOrder.Order = Datalayer.GetOrder(customerDetails.customerId);

                //### only update DeliveryAddress is there is a Order ###
                if (customerOrder.Order is DC_CustomerOrder.DC_Order)
                {
                    customerOrder.Order.DeliveryAddress = string.Format("{0} {1},{2},{3}", customerDetails.houseNumber, customerDetails.street, customerDetails.town, customerDetails.postcode);
                }

                //### update Customer fields ###
                customerOrder.Customer = new DC_CustomerOrder.DC_Customer();
                customerOrder.Customer.FirstName = customerDetails.firstName;
                customerOrder.Customer.LastName = customerDetails.lastName;

                //### return payload ###
                return customerOrder;
            }

            //### return NotFound when customerIDs don't match ###
            return new NotFoundResult();
        }
    }
}
