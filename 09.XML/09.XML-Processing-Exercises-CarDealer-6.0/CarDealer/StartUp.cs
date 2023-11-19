using AutoMapper;
using CarDealer.Data;
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

            //// 10
            //Console.WriteLine(ImportParts(context, partsJson));

            //// 11
            Console.WriteLine(ImportCars(context, carsJson));

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
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

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

            //Deserializing the Xml to Part DTOs
            ImportCarsDTO[] carsDtos = xmlParser.Deserialize<ImportCarsDTO[]>(inputXml, "Cars");

            //Mapping the Car DTOs to Cars only if their parts are unique
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
                    TraveledDistance = dto.traveledDistance
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

            //Adding and Saving
            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(partCars);
            context.SaveChanges();

            //Output
            return $"Successfully imported {cars.Count}";
            }

        }
    }