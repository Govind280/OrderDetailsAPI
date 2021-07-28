using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OderDetailsAPI.Models;
using OderDetailsAPI.Service;
using System;
using System.Threading.Tasks;

namespace OderDetailsAPI.Controllers
{
    /// <summary>
    /// Controller for <see cref="OrderDetailsController"/>
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ICustomerAccountDetailsService _customerAccountDetailsService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly ILogger<OrderDetailsController> _logger;

        /// <summary>
        /// Constructor for <see cref="OrderDetailsController"/>
        /// </summary>
        /// <param name="customerAccountDetailsService"><see cref="ICustomerAccountDetailsService"/></param>
        /// <param name="orderDetailsService"><see cref="IOrderDetailsService"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        public OrderDetailsController(
            ICustomerAccountDetailsService customerAccountDetailsService,
            IOrderDetailsService orderDetailsService,
            ILogger<OrderDetailsController> logger
            )
        {
            _customerAccountDetailsService = customerAccountDetailsService;
            _orderDetailsService = orderDetailsService;
            _logger = logger;
        }

        /// <summary>
        /// Get's Customers recent order details
        /// </summary>
        /// <param name="customerRequestDetails"><see cref="CustomerRequestDetails"/></param>
        /// <returns>Customer recent order details</returns>
        // POST api/<OrderDetails>
        [HttpPost]
        public async Task<IActionResult> RecentOrderDetails([FromBody] CustomerRequestDetails customerRequestDetails)
        {
            try
            {
                if (string.IsNullOrEmpty(customerRequestDetails?.User))
                    return BadRequest("Invalid Customer Request Details. Request object is empty or Email address is empty!!");

                var customerDetails = await _customerAccountDetailsService.GetCustomerDetails(customerRequestDetails.User);

                if (customerDetails == null)
                    return NotFound($"Invalid Customer Email ID. There is no matching customer with {customerRequestDetails.User} in our records!!");

                if (customerDetails.CustomerId != customerRequestDetails.CustomerId)
                    return BadRequest($"Invalid Customer Email ID. {customerRequestDetails.CustomerId} for user {customerRequestDetails.User} " +
                        $"is not matching with CustomerId in our records!!");

                var customerOrderDetails = await _orderDetailsService.GetCustomerRecentOrderDetails(customerDetails);

                return Ok(customerOrderDetails);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Unable to get Order details for customer {customerRequestDetails.User}");
                throw;
            }
        }
    }
}
