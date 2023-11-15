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
            //Console.WriteLine(ImportSuppliers(context, suppliersJson));

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
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CarDealerProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
            }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();
            SupplierDTO[] suppliersDtos = JsonConvert.DeserializeObject<SupplierDTO[]>(inputJson);
            Supplier[] suppliers = mapper.Map<Supplier[]>(suppliersDtos);

            context.Suppliers.AddRangeAsync(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}.";
            }

        public static string ImportParts(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();
            PartsDTO[] partsDtos = JsonConvert.DeserializeObject<PartsDTO[]>(inputJson);
            List<Part> parts = new List<Part>();

            foreach (var part in partsDtos)
                {
                if (context.Suppliers.Any(s => s.Id == part.SupplierId))
                    {
                    parts.Add(mapper.Map<Part>(part));
                    }
                }

            context.Parts.AddRangeAsync(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
            }

        public static string ImportCars(CarDealerContext context, string inputJson)
            {
            IMapper mapper = CreateMapper();
            CarsDTO[] importCarDtos = JsonConvert.DeserializeObject<CarsDTO[]>(inputJson);

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

            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            return $"Successfully imported {carsToAdd.Count}.";
            }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();
            CarsDTO[] customerDtos = JsonConvert.DeserializeObject<CarsDTO[]>(inputJson);
            Customer[] customers = JsonConvert.DeserializeObject<Customer[]>(inputJson);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";
            }

        //13.Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();
            SalesDTO[] salesDtos = JsonConvert.DeserializeObject<SalesDTO[]>(inputJson);
            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
            }
        }
    }