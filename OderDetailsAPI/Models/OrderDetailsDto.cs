using System;
using System.Collections.Generic;

namespace OderDetailsAPI.Models
{
    public class OrderDetailsDto
    {
        public int OrderNumber { get; set; }

        public DateTime? OrderDate { get; set; }

        public string DeliveryAddress { get; set; }

        public List<OrderItemsDto> OrderItems { get; set; }

        public DateTime? DeliveryExpected { get; set; }
    }
}
