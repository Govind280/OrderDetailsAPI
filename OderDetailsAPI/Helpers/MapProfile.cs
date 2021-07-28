using AutoMapper;
using OderDetailsAPI.Data;
using OderDetailsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OderDetailsAPI.Helpers
{
    public class MapProfile : Profile
    {
        /// <summary>
        /// Map Profile for automapper
        /// </summary>
        public MapProfile()
        {
            CreateMap<CustomerDetails, CustomerNameDto>();
            CreateMap<Order, OrderDetailsDto>();
        }
    }
}
