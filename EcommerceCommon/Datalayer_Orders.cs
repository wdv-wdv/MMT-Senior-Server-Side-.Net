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
using EcommerceCommon.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace EcommerceCommon
{
    public class Datalayer_Orders
    {

        private DbContextOrders _Context = null;

        public Datalayer_Orders(string conn)
        {
            var options = new DbContextOptionsBuilder<DbContextOrders>().UseSqlServer(conn ?? throw new ArgumentNullException(nameof(conn))).Options;
            this._Context = new DbContextOrders(options);
        }

        public IQueryable<Order> GetOrder(string customerID)
        {
            var order = _Context.Orders.Where(o => o.Customerid == customerID)
                .Include(o => o.Orderitems)
                .ThenInclude(p => p.Product)
                .OrderByDescending(o => o.Orderdate).Take(1);
            return order;
        }

    }   
}
