using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<ImportSuppliersDTO, Supplier>();

            CreateMap<ImportPartsDTO, Part>();

            CreateMap<ImportCarsDTO, Car>();

            CreateMap<ImportCustomersDTO, Customer>();

            CreateMap<ImportSalesDTO, Sale>();
            }
    }
}
