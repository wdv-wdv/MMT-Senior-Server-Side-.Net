using EcommerceCommon.CustomerModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceCommon
{
    
    public class Datalayer_ClientDetails
    {
        private string Customer_Details_API_Key = null;

        public Datalayer_ClientDetails(string key)
        {
            this.Customer_Details_API_Key = (key ?? throw new ArgumentNullException(nameof(key)));
        }

        public CustomerDetails GetCustomer(string email)
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
                CustomerDetails customer = JsonConvert.DeserializeObject<CustomerDetails>(result);
                return customer;
            }

            //### if HTTPclient request unsuccessful ###
            throw new Exception($"Error retrieving Customer details - API Response: {response.StatusCode}");
        }
    }
}
