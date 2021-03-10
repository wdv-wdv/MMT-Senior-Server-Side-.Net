using EcommerceCommon;
using EcommerceCommon.OrderModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceTest
{
    public static class Test_Helper
    {
        public readonly static Datalayer_Orders _Orders = null;
        public readonly static Datalayer_ClientDetails _ClientDetails = null;
        static Test_Helper()
        {
            if (_Orders == null)
            {
                _Orders = new Datalayer_Orders(Test_Helper.GetConfiguration()["ConnectionStrings:OrderDB"]);
            }

            if (_ClientDetails == null)
            {
                _ClientDetails = new Datalayer_ClientDetails(Test_Helper.GetConfiguration()["API-Key:Customer-Details"]);
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