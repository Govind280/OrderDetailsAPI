using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Models
{
    public class CustomerOrderDetails
    {
        public CustomerNameDto Customer { get; set; }
        public OrderDetailsDto Order { get; set; }
    }
}
