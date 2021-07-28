using Newtonsoft.Json;
using OderDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OderDetailsAPI.Service
{
    public class CustomerAccountDetailsService : ICustomerAccountDetailsService
    {
        private readonly HttpClient _httpClient;

        public CustomerAccountDetailsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerDetails> GetCustomerDetails(string email)
        {
            CustomerDetails customerDetails;
            string url = $"https://customer-account-details.azurewebsites.net/api/GetUserDetails?code=1CrsOooSHlV15C7OYnLY0DHjBHyjzoI8LNHITV04cNCyNCahecPDhw==&email={email}";

            using(var response = await _httpClient.GetAsync(url))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                customerDetails = JsonConvert.DeserializeObject<CustomerDetails>(apiResponse);
            }

            return customerDetails;
        }
    }
}
