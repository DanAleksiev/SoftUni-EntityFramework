using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Castle.Core.Resource;


namespace CarDealer
    {
    public class StartUp
        {
        public static void Main()
            {
            CarDealerContext context = new CarDealerContext();
            string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            string customersJson = File.ReadAllText("../../../Datasets/customers.json");
            string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");

            // 09
            ////Console.WriteLine(ImportSuppliers(context, suppliersJson));

            // 10
            //Console.WriteLine(ImportParts(context, partsJson));

            // 11
            //Console.WriteLine(ImportCars(context, carsJson));

            // 12
            //Console.WriteLine(ImportCustomers(context, customersJson));

            // 13
            Console.WriteLine(ImportSales(context, customersJson));

            }
        public static IMapper CreateMapper()
            {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });



            IMapper mapper = configuration.CreateMapper();

            return mapper;
            }


        public static string ImportSuppliers(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();

            SupplierDTO[] sDtos = JsonConvert.DeserializeObject<SupplierDTO[]>(inputJson);

            Supplier[] suppliers = mapper.Map<Supplier[]>(sDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";

            }

        public static string ImportParts(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();

            PartsDTO[] partsDTOs = JsonConvert.DeserializeObject<PartsDTO[]>(inputJson);
            Part[] parts = mapper.Map<Part[]>(partsDTOs);

            int[] supplierIds = context.Suppliers
                .Select(x => x.Id)
                .ToArray();

            Part[] partsWithvalidSuppliers = parts
                .Where(p => supplierIds.Contains(p.SupplierId)).ToArray();

            context.Parts.AddRange(partsWithvalidSuppliers);
            context.SaveChanges();

            return $"Successfully imported {partsWithvalidSuppliers.Count()}.";
            }

        public static string ImportCars(CarDealerContext context, string inputJson)
            {
            IMapper mapper = CreateMapper();

            //Turning the json file to a DTO
            CarsDTO[] importCarDtos = JsonConvert.DeserializeObject<CarsDTO[]>(inputJson);

            //Mapping the Cars from their DTOs
            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var carDto in importCarDtos)
                {
                Car currentCar = mapper.Map<Car>(carDto);

                foreach (var id in carDto.PartsId)
                    {
                    if (context.Parts.Any(p => p.Id == id))
                        {
                        currentCar.PartsCars.Add(new PartCar
                            {
                            PartId = id,
                            });
                        }
                    }

                carsToAdd.Add(currentCar);
                }

            //Adding the Cars
            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            //Output
            return $"Successfully imported {carsToAdd.Count}.";
            }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();
            SalesDTO[] customersCarDtos = JsonConvert.DeserializeObject<SalesDTO[]>(inputJson);
            Sale[] customers = mapper.Map<Sale[]>(customersCarDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
            }

        public static string ImportSales(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();
            SalesDTO[] salesCarDtos = JsonConvert.DeserializeObject<SalesDTO[]>(inputJson);
            Sale[] sales = mapper.Map<Sale[]>(salesCarDtos);

            context.Customers.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
            }
        }
    }