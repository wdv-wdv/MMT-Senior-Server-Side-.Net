using EcommerceCommon;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceTest
{
    class Test_Helper
    {
        public static void Init()
        {
            if (Datalayer.OrderDB == null)
            {
                Datalayer.OrderDB = new MSSQL_helper(Test_Helper.GetConfiguration()["ConnectionStrings:OrderDB"]);
            }

            if (Datalayer.Customer_Details_API_Key == null)
            {
                Datalayer.Customer_Details_API_Key = Test_Helper.GetConfiguration()["API-Key:Customer-Details"];
            }
        }


        public static IConfigurationRoot GetConfiguration()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

            return config;

        }
    }
}