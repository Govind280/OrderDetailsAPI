using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OderDetailsAPI.Models;
using OderDetailsAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OderDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ICustomerAccountDetailsService _customerAccountDetailsService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly ILogger<OrderDetailsController> _logger;

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
