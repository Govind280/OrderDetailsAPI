using OderDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Service
{
    /// <summary>
    /// Interface for <see cref="IOrderDetailsService"/>
    /// </summary>
    public interface IOrderDetailsService
    {
        /// <summary>
        /// Get's Customer recent order details
        /// </summary>
        /// <param name="customerDetails"><see cref="CustomerDetails"/></param>
        /// <returns><see cref="CustomerOrderDetails"/></returns>
        Task<CustomerOrderDetails> GetCustomerRecentOrderDetails(CustomerDetails customerDetails);
    }
}
