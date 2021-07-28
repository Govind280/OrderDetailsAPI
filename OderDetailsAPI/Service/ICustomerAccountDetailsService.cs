using OderDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Service
{
    /// <summary>
    /// Interface for <see cref="ICustomerAccountDetailsService"/>
    /// </summary>
    public interface ICustomerAccountDetailsService
    {
        /// <summary>
        /// Get customer details
        /// </summary>
        /// <param name="email">Customer's email address</param>
        /// <returns><see cref="CustomerDetails"/></returns>
        Task<CustomerDetails> GetCustomerDetails(string email);
    }
}
