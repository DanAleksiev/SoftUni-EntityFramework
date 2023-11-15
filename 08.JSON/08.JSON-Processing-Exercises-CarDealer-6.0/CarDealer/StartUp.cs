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
            string salesJson = File.ReadAllText("../../../Datasets/sales.json");
            string carsJson = File.ReadAllText("../../../Datasets/cars.json");
            string partsJson = File.ReadAllText("../../../Datasets/parts.json");
            string customersJson = File.ReadAllText("../../../Datasets/customers.json");
            string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.json");

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //// 09
            //Console.WriteLine(ImportSuppliers(context, suppliersJson));

            //// 10
            //Console.WriteLine(ImportParts(context, partsJson));

            //// 11
            //Console.WriteLine(ImportCars(context, carsJson));

            //// 12
            //Console.WriteLine(ImportCustomers(context, customersJson));

            //// 13
            //Console.WriteLine(ImportSales(context, salesJson));

            // 14
            //Console.WriteLine(GetOrderedCustomers(context));

            // 15
            //Console.WriteLine(GetCarsFromMakeToyota(context));

            // 16
            //Console.WriteLine(GetLocalSuppliers(context));

            // 17
            Console.WriteLine(GetCarsWithTheirListOfParts(context));
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

            CarsDTO[] importCarDtos = JsonConvert.DeserializeObject<CarsDTO[]>(inputJson);
            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var carDto in importCarDtos)
                {
                Car currentCar = mapper.Map<Car>(carDto);

                foreach (var id in carDto.PartsIds)
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
            CustomersDTO[] customersCarDtos = JsonConvert.DeserializeObject<CustomersDTO[]>(inputJson);
            Customer[] customers = mapper.Map<Customer[]>(customersCarDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
            }

        public static string ImportSales(CarDealerContext context, string inputJson)
            {
            var mapper = CreateMapper();
            SalesDTO[] salesDtos = JsonConvert.DeserializeObject<SalesDTO[]>(inputJson);
            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
            }

        public static string GetOrderedCustomers(CarDealerContext context)
            {
            var result = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                    {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = x.IsYoungDriver
                    });

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            File.WriteAllText(@"../../../Results/ordered-customers.json", json);

            return json.Trim();
            }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
            {
            var result = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TraveledDistance)
                .Select(x => new
                    {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TraveledDistance,
                    });

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            File.WriteAllText(@"../../../Results/toyota-cars.json", json);

            return json.Trim();
            }

        public static string GetLocalSuppliers(CarDealerContext context)
            {
            var result = context.Suppliers
               .Where(x => x.IsImporter == false)
               .Select(x => new
                   {
                   x.Id,
                   x.Name,
                   PartsCount = x.Parts.Count,
                   });

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            File.WriteAllText(@"../../../Results/local-suppliers.json", json);

            return json.Trim();
            }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
            {
            var result = context.Cars
              .Select(x => new
                  {
                  x.Make,
                  x.Model,
                  x.TraveledDistance,
                  parts = x.PartsCars.Select(y => new
                      {
                      Name = y.Part.Name,
                      Price = y.Part.Price.ToString("f2"),
                      }).ToArray(),
                  })
              .ToArray();

            var json = JsonConvert.SerializeObject(result, Formatting.Indented);

            File.WriteAllText(@"../../../Results/cars-and-parts.json", json);

            return json.Trim();
            }
        }
    }