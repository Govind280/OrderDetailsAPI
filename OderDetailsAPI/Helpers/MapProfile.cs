using AutoMapper;
using OderDetailsAPI.Data;
using OderDetailsAPI.Models;

namespace OderDetailsAPI.Helpers
{
    /* To be implemented in Future */
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
