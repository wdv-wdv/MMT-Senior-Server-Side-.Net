using EcommerceAPI.Controllers;
using EcommerceCommon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EcommerceTest
{
    [TestClass]
    public class EcommerceOrders
    {
        public EcommerceOrders()
        {
            Test_Helper.Init();
        }

        [DataRow("C34454")]
        [DataRow("R34788")]
        [DataRow("A99001")]
        [DataRow("XM45001")]
        [DataTestMethod]
        public void Datalayer_GetOrder(string customerID)
        {
            DC_CustomerOrder.DC_Order order = Datalayer.GetOrder(customerID);

            //### Should pass if DC_Order object is returned ###
            Assert.IsTrue(order is DC_CustomerOrder.DC_Order);
            Debug.Print(JsonConvert.SerializeObject(order));
        }

        [DataRow("cat.owner@mmtdigital.co.uk","C34454")]
        [DataRow("dog.owner@fake-customer.com","R34788")]
        [DataRow("sneeze@fake-customer.com","A99001")]
        [DataRow("santa@north-pole.lp.com","XM45001")]
        [DataTestMethod]
        public void Datalayer_GetCustomer(string email, string customerID)
        {
            var customer = Datalayer.GetCustomer(email);

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
            var controller = new OrderController();
            var result = controller.LastOrders(new DC_LastOrdersPostRequest() { User = email, CustomerID = customerID });

            //### Should pass if DC_CustomerOrder object is returned ###
            Assert.IsTrue(result is DC_CustomerOrder);
            Debug.Print(JsonConvert.SerializeObject(result));
        }

        [DataRow("cat.owner@mmtdigital.co.uk", "XXXXX")]
        [DataRow("dog.owner@fake-customer.com", "XXXXX")]
        [DataRow("sneeze@fake-customer.com", "XXXXX")]
        [DataRow("santa@north-pole.lp.com", "XXXXX")]
        [DataTestMethod]
        public void API_LastOrders_Invalid_CustomerID(string email, string customerID)
        {
            var controller = new OrderController();
            var result = controller.LastOrders(new DC_LastOrdersPostRequest() { User = email, CustomerID = customerID });

            //### Should pass if NotFound is returned ###
            Assert.IsTrue(result is NotFoundResult);
        }
    }
}
