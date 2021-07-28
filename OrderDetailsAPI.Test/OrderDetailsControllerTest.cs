using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OderDetailsAPI.Controllers;
using OderDetailsAPI.Models;
using OderDetailsAPI.Service;
using System;
using System.Threading.Tasks;

namespace OrderDetailsAPI.Test
{
    [TestClass]
    public class OrderDetailsControllerTest
    {
        private Mock<ICustomerAccountDetailsService> _mockCustomerAccountDetailsService;
        private Mock<IOrderDetailsService> _mockOrderDetailsService;
        private Mock<ILogger<OrderDetailsController>> _mockLogger;
        private OrderDetailsController _orderDetailsController;

        [TestInitialize]
        public void Init()
        {
            _mockCustomerAccountDetailsService = new Mock<ICustomerAccountDetailsService>();
            _mockOrderDetailsService = new Mock<IOrderDetailsService>();
            _mockLogger = new Mock<ILogger<OrderDetailsController>>();
            _orderDetailsController = new OrderDetailsController(_mockCustomerAccountDetailsService.Object, _mockOrderDetailsService.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task RecentOrderDetails_Invalid_WithoutCustomerEmail()
        {
            // Arrange
            var customerRequestDetails = GetValidCustomerRequestDetails();
            customerRequestDetails.User = null;

            // Act
            var result = await _orderDetailsController.RecentOrderDetails(customerRequestDetails);

            // Assert
            BadRequestObjectResult actionResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.AreEqual($"Invalid Customer Request Details. Request object is empty or Email address is empty!!", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task RecentOrderDetails_Invalid_Customer_NotAvailable()
        {
            // Arrange
            var customerRequestDetails = GetValidCustomerRequestDetails();
            CustomerDetails customerDetails = null;

            _mockCustomerAccountDetailsService.Setup(a => a.GetCustomerDetails(customerRequestDetails.User)).ReturnsAsync(customerDetails);

            // Act
            var result = await _orderDetailsController.RecentOrderDetails(customerRequestDetails);

            // Assert
            NotFoundObjectResult actionResult = result as NotFoundObjectResult;
            Assert.AreEqual(404, actionResult.StatusCode);
            Assert.AreEqual($"Invalid Customer Email ID. There is no matching customer with {customerRequestDetails.User} in our records!!", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task RecentOrderDetails_Invalid_CustomerID_NotMatching()
        {
            // Arrange
            var customerRequestDetails = GetValidCustomerRequestDetails();

            _mockCustomerAccountDetailsService.Setup(a => a.GetCustomerDetails(customerRequestDetails.User)).ReturnsAsync(new CustomerDetails()
            {
                CustomerId = "T5678"
            });

            // Act
            var result = await _orderDetailsController.RecentOrderDetails(customerRequestDetails);

            // Assert
            BadRequestObjectResult actionResult = result as BadRequestObjectResult;
            Assert.AreEqual(400, actionResult.StatusCode);
            Assert.AreEqual($"Invalid Customer Email ID. {customerRequestDetails.CustomerId} for user {customerRequestDetails.User} " +
                        $"is not matching with CustomerId in our records!!", actionResult.Value);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task RecentOrderDetails_Valid_Test()
        {
            // Arrange
            var customerRequestDetails = GetValidCustomerRequestDetails();
            var customerDetails = new CustomerDetails()
            {
                Email = "testemail@test.com",
                CustomerId = "T12345",
                website = true,
                FirstName = "Charlie",
                LastName = "Cat",
                LastLoggedIn = DateTime.Now,
                HouseNumber = "1a",
                Street = "Uppingham Gate",
                Town = "Uppingham",
                Postcode = "LE15 9NY",
                PreferredLanguage = "en-gb"
            };
            var expectedDeliveryDate = DateTime.Now.AddDays(10);

            _mockCustomerAccountDetailsService.Setup(a => a.GetCustomerDetails(customerRequestDetails.User)).ReturnsAsync(customerDetails);

            _mockOrderDetailsService.Setup(x => x.GetCustomerRecentOrderDetails(customerDetails)).ReturnsAsync(new CustomerOrderDetails()
            {
                Customer = new()
                {
                    FirstName = customerDetails.FirstName,
                    LastName = customerDetails.LastName
                },
                Order = new()
                {
                    DeliveryAddress = ($"{customerDetails.HouseNumber}, {customerDetails.Street}, {customerDetails.Town}, {customerDetails.Postcode}"),
                    DeliveryExpected = expectedDeliveryDate,
                    OrderDate = DateTime.Now.AddDays(-1),
                    OrderNumber = 123
                }
            });            

            // Act
            var result = await _orderDetailsController.RecentOrderDetails(customerRequestDetails);

            // Assert
            OkObjectResult actionResult = result as OkObjectResult;
            CustomerOrderDetails actualResultValue = (CustomerOrderDetails)actionResult.Value;
            Assert.AreEqual(200, actionResult.StatusCode);
            Assert.AreEqual(expectedDeliveryDate, actualResultValue.Order.DeliveryExpected);
            Assert.AreEqual(123, actualResultValue.Order.OrderNumber);
            Assert.AreEqual(customerDetails.FirstName, actualResultValue.Customer.FirstName);
            Assert.AreEqual(($"{customerDetails.HouseNumber}, {customerDetails.Street}, {customerDetails.Town}, {customerDetails.Postcode}"), 
                actualResultValue.Order.DeliveryAddress);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        private CustomerRequestDetails GetValidCustomerRequestDetails() => new()
        {
            CustomerId = "T12345",
            User = "testemail@test.com"
        };
    }
}
