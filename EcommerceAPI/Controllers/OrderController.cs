using EcommerceAPI.Models;
using EcommerceCommon;
using EcommerceCommon.CustomerModels;
using Microsoft.AspNetCore.Http;
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

        private readonly Datalayer_Orders _Orders;
        private readonly Datalayer_ClientDetails _ClientDetails;

        public OrderController(Datalayer_Orders orders, Datalayer_ClientDetails clientDetails)
        {
            _Orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _ClientDetails = clientDetails ?? throw new ArgumentNullException(nameof(clientDetails));
        }

        [HttpPost("LastOrders")]
        public object LastOrders([FromBody] LastOrdersPostRequest lastOrdersPostRequest)
        {
            try
            {

                CustomerDetails customerDetails = _ClientDetails.GetCustomer(lastOrdersPostRequest.User);

                //### check customerIDs match ###
                if (customerDetails is CustomerDetails && customerDetails.customerId.Equals(lastOrdersPostRequest.CustomerID, StringComparison.OrdinalIgnoreCase))
                {
                    //### Create payload object ###
                    var order = _Orders.GetOrder(customerDetails.customerId);
                    var customerOrder = new
                    {
                        //### Order details ###
                        Order = order.Select(o => new
                        {
                            OrderNumber = o.Orderid,
                            OrderDate = o.Orderdate,
                            DeliveryAddress = string.Format("{0} {1},{2},{3}", customerDetails.houseNumber, customerDetails.street, customerDetails.town, customerDetails.postcode),
                            DeliveryExpected = o.Deliveryexpected,
                            //### Order items ###
                            OrderItems = o.Orderitems.Select(i => new
                            {
                                //### Is a gift logic
                                Product = o.Containsgift == true ? "Gift" : String.Join(" ", new string[] { i.Product.Productname, i.Product.Colour, i.Product.Size }).Trim(),
                                Quantity = i.Quantity,
                                PriceEach = i.Price
                            }).ToList()
                        }).ToList(),
                        //### Customer details ###
                        Customer = new
                        {
                            FirstName = customerDetails.firstName,
                            LastName = customerDetails.lastName
                        }
                    };

                    //### return payload ###
                    return customerOrder;
                }

                //### return NotFound when customerIDs don't match ###
                return new NotFoundResult();
            }
            catch (Exception)
            {
                //### Likely add some sort of logging here ###

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
