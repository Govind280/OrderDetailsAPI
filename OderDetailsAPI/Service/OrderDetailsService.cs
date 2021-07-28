using Microsoft.EntityFrameworkCore;
using OderDetailsAPI.Data;
using OderDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Service
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private SSE_TestContext _sSE_TestContext;

        public OrderDetailsService(SSE_TestContext sSE_TestContext)
        {
            _sSE_TestContext = sSE_TestContext;
        }

        public async Task<CustomerOrderDetails> GetCustomerRecentOrderDetails(CustomerDetails customerDetails)
        {

            var order = await _sSE_TestContext.Orders.Where(a => a.Customerid == customerDetails.CustomerId)
                                                     .OrderByDescending(x => x.Orderdate).FirstOrDefaultAsync();

            if (order == null)
                return BuildCustomerDetails(customerDetails);

            var orderitems = await _sSE_TestContext.Orderitems.Where(a => a.Orderid == order.Orderid)
                                                              .Include(x => x.Product)
                                                              .ToListAsync();

            return BuildCustomerDetails(customerDetails, order, orderitems);


            throw new NotImplementedException();
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
            if (orderitems != null && orderitems.Any())
            {
                List<OrderItemsDto> orderItemsdtoCollection = new List<OrderItemsDto>();

                OrderItemsDto orderItemsDto;

                foreach (var item in orderitems)
                {
                    orderItemsDto = new()
                    {
                        Product = containsGift.HasValue && containsGift.Value ? "Gift" : item.Product?.Productname,
                        PriceEach = item.Price,
                        Quantity = item.Quantity
                    };

                    orderItemsdtoCollection.Add(orderItemsDto);
                }

                return orderItemsdtoCollection;
            }
            else
            {
                return null;
            }
        }
    }
}
