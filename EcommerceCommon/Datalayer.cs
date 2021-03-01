using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using static EcommerceCommon.DC_CustomerOrder.DC_Order;

namespace EcommerceCommon
{
    public static class Datalayer
    {
        public static string Customer_Details_API_Key = null;
        public static MSSQL_helper OrderDB = null;

        public static DC_CustomerOrder.DC_Order GetOrder(string customerID)
        {
            //### Run query ###
            string sql = @"Declare @orderID as int;
                           Select top 1 @orderID=ORDERID from ORDERS where CUSTOMERID = @CUSTOMERID Order by ORDERDATE desc ;
                           Select ORDERID, ORDERDATE, DELIVERYEXPECTED, CONTAINSGIFT from ORDERS where ORDERID = @orderID ;
                           Select I.QUANTITY, I.PRICE, P.PRODUCTNAME, P.COLOUR, P.SIZE
                           from ORDERS O
                           join ORDERITEMS I on O.ORDERID = I.ORDERID And O.ORDERID = @orderID
                           join PRODUCTS P on P.PRODUCTID = I.PRODUCTID";
            DataTable[] tables = OrderDB.RunQueryStatement(sql, new SqlParameter("@CUSTOMERID", customerID));

            //### Check if any order was retrieve, else return null ###
            if(tables[0].Rows.Count > 0)
            {
                //### Populate Order object ###
                DC_CustomerOrder.DC_Order order = new DC_CustomerOrder.DC_Order();
                order.OrderNumber = tables[0].Rows[0].Field<int>("ORDERID");
                order.OrderDate = tables[0].Rows[0].Field<DateTime>("ORDERDATE").Date;
                order.DeliveryExpected = tables[0].Rows[0].Field<DateTime>("DELIVERYEXPECTED").Date;

                Boolean gift = tables[0].Rows[0].Field<Boolean>("CONTAINSGIFT");

                //### Populate OrderItems ###
                if (tables[1].Rows.Count > 0)
                {
                    order.OrderItems = new List<DC_CustomerOrder.DC_Order.DC_OrderItem>();
                    foreach (DataRow row in tables[1].Rows)
                    {
                        DC_OrderItem item = new DC_OrderItem();
                        item.Quantity = row.Field<int>("QUANTITY");
                        item.PriceEach = (row["PRICE"] is DBNull ? null : row.Field<Decimal?>("PRICE"));

                        //### If gift change Product to "Gift" ###
                        if (gift) {
                            item.Product = "Gift";
                        } else {
                            item.Product = String.Join(" ",new string[] {
                                row.Field<string>("PRODUCTNAME"),
                                row["COLOUR"] is DBNull ? null : row.Field<string>("COLOUR"),
                                row["SIZE"] is DBNull ? null : row.Field<string>("SIZE")
                            }.Where(s => s is string));
                        }
                        order.OrderItems.Add(item);
                    }
                }

                //### Return order object ###
                return order;
            }

            //### Return null if no orders ###
            return null;
        }

        public static DC_CustomerDetails GetCustomer(string email)
        {
            //### Make HTTP request ##
            Uri uri = new Uri($"https://customer-details.azurewebsites.net/api/GetUserDetails?code={Customer_Details_API_Key}&email={email}");
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = Task.Run(async () => await httpClient.GetAsync(uri)).Result;

            //### Check response success, else throw exception ###
            if (response.IsSuccessStatusCode)
            {
                //### Populate Customer Details object ###
                string result = Task.Run(async () => await response.Content.ReadAsStringAsync()).Result;
                DC_CustomerDetails customer = JsonConvert.DeserializeObject<DC_CustomerDetails>(result);
                return customer;
            }

            //### if HTTPclient request unsuccessful ###
            throw new Exception($"Error retrieving Customer details - API Response: {response.StatusCode}");
        }
    }
}
