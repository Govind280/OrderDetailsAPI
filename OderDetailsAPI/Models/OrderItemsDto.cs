using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Models
{
    public class OrderItemsDto
    {
        public string Product { get; set; }

        public int? Quantity { get; set; }

        public decimal? PriceEach { get; set; }
    }
}
