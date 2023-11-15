using AutoMapper;
using CarDealer.Models;
using CarDealer.DTOs;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierDTO, Supplier>();

            CreateMap<PartsDTO, Part>();

            CreateMap<CarsDTO, Car>();

            CreateMap<SalesDTO, Sale>();
            }
    }
}
