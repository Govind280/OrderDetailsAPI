using OderDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Service
{
    public interface IOrderDetailsService
    {
        Task<CustomerOrderDetails> GetCustomerRecentOrderDetails(CustomerDetails customerDetails);
    }
}
