using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CustomerAccountDetailsService> _logger;

        public CustomerAccountDetailsService(HttpClient httpClient, ILogger<CustomerAccountDetailsService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CustomerDetails> GetCustomerDetails(string email)
        {
            try
            {
                CustomerDetails customerDetails;
                string url = $"https://customer-account-details.azurewebsites.net/api/GetUserDetails?code=1CrsOooSHlV15C7OYnLY0DHjBHyjzoI8LNHITV04cNCyNCahecPDhw==&email={email}";

                using (var response = await _httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    customerDetails = JsonConvert.DeserializeObject<CustomerDetails>(apiResponse);
                }

                return customerDetails;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Exception while Fetching customer details for user {email}");
                throw;
            }
        }
    }
}
