using AutoMapper;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
    {
    public class CarDealerProfile : Profile
        {
        public CarDealerProfile()
            {
            CreateMap<ImportSuppliersDTO, Supplier>();
            CreateMap<Supplier, ExportSuppliersWhoDontImportDTO>();

            CreateMap<ImportPartsDTO, Part>();

            CreateMap<ImportCarsDTO, Car>();
            CreateMap<Car, ExportCarsDTO>();
            CreateMap<Car, ExportCarsBMWDTO>();

            CreateMap<ImportCustomersDTO, Customer>();

            CreateMap<ImportSalesDTO, Sale>();
            }
        }
    }
