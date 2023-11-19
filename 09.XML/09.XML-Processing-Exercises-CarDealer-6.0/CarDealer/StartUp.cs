using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using ProductShop;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer
    {
    public class StartUp
        {
        public static void Main()
            {
            CarDealerContext context = new CarDealerContext();
            string salesJson = File.ReadAllText("../../../Datasets/sales.xml");
            string carsJson = File.ReadAllText("../../../Datasets/cars.xml");
            string partsJson = File.ReadAllText("../../../Datasets/parts.xml");
            string customersJson = File.ReadAllText("../../../Datasets/customers.xml");
            string suppliersJson = File.ReadAllText("../../../Datasets/suppliers.xml");

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //// 09
            //Console.WriteLine(ImportSuppliers(context, suppliersJson));

            ////10
            //Console.WriteLine(ImportParts(context, partsJson));

            ////11
            //Console.WriteLine(ImportCars(context, carsJson));

            ////12
            //Console.WriteLine(ImportCustomers(context, customersJson));

            ////13
            //Console.WriteLine(ImportSales(context, salesJson));

            // 14
            //Console.WriteLine(GetCarsWithDistance(context));

            // 15
            //Console.WriteLine(GetCarsFromMakeBmw(context));

            //16
            //Console.WriteLine(GetLocalSuppliers(context));

            // 17
            Console.WriteLine(GetCarsWithTheirListOfParts(context));

            // 18
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            // 19
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));
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

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
            {
            XmlFormating formating = new XmlFormating();
            ImportSuppliersDTO[] dto = formating.Deserialize<ImportSuppliersDTO[]>(inputXml, "Suppliers");
            // 2.read the xml
            // 3.map the object
            var map = CreateMapper();
            Supplier[] output = map.Map<Supplier[]>(dto);

            // 4. commit to db
            context.Suppliers.AddRange(output);
            // 5. save changes thu
            context.SaveChanges();

            return $"Successfully imported {output.Length}";
            }

        public static string ImportParts(CarDealerContext context, string inputXml)
            {
            XmlFormating formating = new XmlFormating();
            ImportPartsDTO[] dto = formating.Deserialize<ImportPartsDTO[]>(inputXml, "Parts");

            var map = CreateMapper();
            Part[] output = map.Map<Part[]>(dto);


            List<Part> parts = new List<Part>();
            var suppliers = context.Suppliers;
            foreach (var part in output)
                {
                foreach (var supplier in suppliers)
                    {
                    if (part.SupplierId == supplier.Id)
                        {
                        part.Supplier = supplier;
                        parts.Add(part);
                        }
                    }
                }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
            }


        public static string ImportCars(CarDealerContext context, string inputXml)
            {
            var mapper = CreateMapper();
            var xmlParser = new XmlFormating();

            ImportCarsDTO[] carsDtos = xmlParser.Deserialize<ImportCarsDTO[]>(inputXml, "Cars");

            List<Car> cars = new List<Car>();
            List<PartCar> partCars = new List<PartCar>();
            int[] allPartIds = context.Parts.Select(p => p.Id).ToArray();
            int carId = 1;

            foreach (var dto in carsDtos)
                {
                Car car = new Car()
                    {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance
                    };

                cars.Add(car);

                foreach (int partId in dto.Parts
                    .Where(p => allPartIds.Contains(p.Id))
                    .Select(p => p.Id)
                    .Distinct())
                    {
                    PartCar partCar = new PartCar()
                        {
                        CarId = carId,
                        PartId = partId
                        };
                    partCars.Add(partCar);
                    }
                carId++;
                }

            //Adding and Savingz
            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partCars);
            context.SaveChanges();

            //Output
            return $"Successfully imported {cars.Count}";
            }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
            {
            XmlFormating formating = new XmlFormating();
            ImportCustomersDTO[] dto = formating.Deserialize<ImportCustomersDTO[]>(inputXml, "Customers");

            var map = CreateMapper();
            Customer[] output = map.Map<Customer[]>(dto);

            context.Customers.AddRange(output);
            context.SaveChanges();

            return $"Successfully imported {output.Length}";
            }

        public static string ImportSales(CarDealerContext context, string inputXml)
            {
            XmlFormating formating = new XmlFormating();
            ImportSalesDTO[] dto = formating.Deserialize<ImportSalesDTO[]>(inputXml, "Sales");


            var cars = context.Cars.Select(x => x.Id).ToArray();

            var map = CreateMapper();
            Sale[] output = map.Map<Sale[]>(dto).Where(x => cars.Contains(x.CarId)).ToArray();


            context.Sales.AddRange(output);
            context.SaveChanges();

            return $"Successfully imported {output.Length}";
            }

        public static string GetCarsWithDistance(CarDealerContext context)
            {
            var map = CreateMapper();

            var result = context.Cars
                .Where(x => x.TraveledDistance > 2000000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .ProjectTo<ExportCarsDTO>(map.ConfigurationProvider)
                .ToArray();

            XmlFormating formating = new XmlFormating();
            return formating.Serialize<ExportCarsDTO[]>(result, "cars");
            }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
            {
            var map = CreateMapper();

            var result = context.Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TraveledDistance)
                .ProjectTo<ExportCarsBMWDTO>(map.ConfigurationProvider)
                .ToArray();

            XmlFormating formating = new XmlFormating();
            return formating.Serialize<ExportCarsBMWDTO[]>(result, "cars");
            }

        public static string GetLocalSuppliers(CarDealerContext context)
            {
            var map = CreateMapper();
            var result = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new ExportSuppliersWhoDontImportDTO
                    {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                    })
                .ToArray();


            XmlFormating formating = new XmlFormating();
            return formating.Serialize<ExportSuppliersWhoDontImportDTO[]>(result, "suppliers");
            }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
            {
            var map = CreateMapper();
            var result = context.Cars
                .Select(x => new ExportCarsWithTheyrPartsDTO()
                    {
                    Make = x.Make,
                    Model = x.Model,
                    TraveledDistance = x.TraveledDistance,
                    Parts = x.PartsCars.Select(p => new CarParts(){
                        Price = p.Part.Price,
                        Name = p.Part.Name
                        })
                    .OrderByDescending(x => x.Price)
                    .ToArray()
                    })
                .OrderByDescending(x=>x.TraveledDistance)
                .ThenBy(x=>x.Model)
                .Take(5)
                .ToArray();


        XmlFormating formating = new XmlFormating();
            return formating.Serialize<ExportCarsWithTheyrPartsDTO[]>(result, "cars");
            }
    }
    }