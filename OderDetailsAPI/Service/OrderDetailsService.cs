using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OderDetailsAPI.Data;
using OderDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Service
{
    /// <summary>
    /// Service class for <see cref="OrderDetailsService"/>
    /// </summary>
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly SSE_TestContext _sSE_TestContext;
        private readonly ILogger<OrderDetailsService> _logger;

        /// <summary>
        /// Constructor for <see cref="OrderDetailsService"/>
        /// </summary>
        /// <param name="sSE_TestContext"><see cref="SSE_TestContext"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        public OrderDetailsService(SSE_TestContext sSE_TestContext, ILogger<OrderDetailsService> logger)
        {
            _sSE_TestContext = sSE_TestContext;
            _logger = logger;
        }

        /// <summary>
        /// Get's Customer recent order details
        /// </summary>
        /// <param name="customerDetails"><see cref="CustomerDetails"/></param>
        /// <returns><see cref="CustomerOrderDetails"/></returns>
        public async Task<CustomerOrderDetails> GetCustomerRecentOrderDetails(CustomerDetails customerDetails)
        {
            try
            {
                var order = await _sSE_TestContext.Orders.Where(a => a.Customerid == customerDetails.CustomerId)
                                                         .OrderByDescending(x => x.Orderdate).FirstOrDefaultAsync();

                if (order == null)
                    return BuildCustomerDetails(customerDetails);

                var orderitems = await _sSE_TestContext.Orderitems.Where(a => a.Orderid == order.Orderid)
                                                                  .Include(x => x.Product)
                                                                  .ToListAsync();

                return BuildCustomerDetails(customerDetails, order, orderitems);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Exception while Fetching order details for user {customerDetails?.CustomerId}");
                throw;
            }
        }

        private CustomerOrderDetails BuildCustomerDetails(CustomerDetails customerDetails, Order order = null, List<Orderitem> orderitems = null) => new()
        {
            Customer = new()
            {
                FirstName = customerDetails.FirstName,
                LastName = customerDetails.LastName
            },
            Order = order == null ? null : new()
            {
                OrderNumber = order.Orderid,
                OrderDate = order.Orderdate,
                DeliveryAddress = ($"{customerDetails.HouseNumber}, {customerDetails.Street}, {customerDetails.Town}, {customerDetails.Postcode}"),
                DeliveryExpected = order.Deliveryexpected,
                OrderItems = MapOrderItems(orderitems, order.Containsgift)
            }
        };

        private List<OrderItemsDto> MapOrderItems(List<Orderitem> orderitems, bool? containsGift)
        {
            if (orderitems == null || !orderitems.Any())
                return null;
            
            List<OrderItemsDto> orderItemsdtoCollection = new List<OrderItemsDto>();

            foreach (var item in orderitems)
            {
                OrderItemsDto orderItemsDto = new()
                {
                    Product = containsGift.HasValue && containsGift.Value ? "Gift" : item.Product?.Productname,
                    PriceEach = item.Price,
                    Quantity = item.Quantity
                };

                orderItemsdtoCollection.Add(orderItemsDto);
            }

            return orderItemsdtoCollection;
        }
    }
}
