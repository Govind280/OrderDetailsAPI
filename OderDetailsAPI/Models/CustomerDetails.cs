using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Models
{
    public class CustomerDetails
    {
        public string Email { get; set; }
        public string CustomerId { get; set; }
        public bool website { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
