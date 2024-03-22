using EcommerceAPI.Controllers;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EcommerceTest
{
    [TestClass]
    public class EcommerceOrders
    {

        [DataRow("C34454")]
        [DataRow("R34788")]
        [DataRow("A99001")]
        [DataRow("XM45001")]
        [DataTestMethod]
        public void Datalayer_GetOrder(string customerID)
        {
            var order = Test_Helper._Orders.GetOrder(customerID);

            //### Should pass if object is not null ###
            Assert.IsNotNull(order);
        }

        [DataRow("cat.owner@mmtdigital.co.uk", "C34454")]
        [DataRow("dog.owner@fake-customer.com", "R34788")]
        [DataRow("sneeze@fake-customer.com", "A99001")]
        [DataRow("santa@north-pole.lp.com", "XM45001")]
        [DataTestMethod]
        public void Datalayer_GetCustomer(string email, string customerID)
        {
            var customer = Test_Helper._ClientDetails.GetCustomer(email);

            //### Should pass if CustomerIDs match ###
            Assert.IsTrue(customer.customerId.Equals(customerID, System.StringComparison.OrdinalIgnoreCase));

        }

        [DataRow("cat.owner@mmtdigital.co.uk", "C34454")]
        [DataRow("dog.owner@fake-customer.com", "R34788")]
        [DataRow("sneeze@fake-customer.com", "A99001")]
        [DataRow("santa@north-pole.lp.com", "XM45001")]
        [DataTestMethod]
        public void API_LastOrders(string email, string customerID)
        {
            var controller = new OrderController(Test_Helper._Orders,Test_Helper._ClientDetails);
            var result = controller.LastOrders(new LastOrdersPostRequest() { User = email, CustomerID = customerID });

            //### Is anything return?? ###
            Assert.IsNotNull(result);

            //## Check properties exist ###
            Assert.IsNotNull(result.GetType().GetProperty("Order"));
            Assert.IsNotNull(result.GetType().GetProperty("Customer"));
            //### etc, etc for all the remaining properties ... ###
        }

        [DataRow("cat.owner@mmtdigital.co.uk", "XXXXX")]
        [DataRow("dog.owner@fake-customer.com", "XXXXX")]
        [DataRow("sneeze@fake-customer.com", "XXXXX")]
        [DataRow("santa@north-pole.lp.com", "XXXXX")]
        [DataTestMethod]
        public void API_LastOrders_Invalid_CustomerID(string email, string customerID)
        {
            var controller = new OrderController(Test_Helper._Orders, Test_Helper._ClientDetails);
            var result = controller.LastOrders(new LastOrdersPostRequest() { User = email, CustomerID = customerID });

            //### Should pass if NotFound is returned because customerID doesn't match the email ###
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
