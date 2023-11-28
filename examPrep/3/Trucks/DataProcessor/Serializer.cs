namespace Trucks.DataProcessor
    {
    using Data;
    using Invoices.Extentions;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
        {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
            {
            var result = context.Despatchers
                .Where(d => d.Trucks.Any())
                .Select(d => new ExportDespatchersWithTheirTrucksDTO()
                    {
                    Count = d.Trucks.Count(),
                    DespatcherName = d.Name,
                    Trucks = d.Trucks.Select(t => new AllDTrucks()
                        {
                        RegistrationNumber = t.RegistrationNumber,
                        Make = t.MakeType
                        })
                    .OrderBy(t => t.RegistrationNumber)
                    .ToArray()
                    })
                .OrderByDescending(d=>d.Trucks.Count())
                .ThenBy(d=>d.DespatcherName)
                .ToArray();

            XmlFormating format = new XmlFormating();

            return format.Serialize<ExportDespatchersWithTheirTrucksDTO[]>(result, "Despatchers");
            }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
            {
            var result = context.Clients
                .Where(x => x.ClientsTrucks.Any(c => c.Truck.TankCapacity >= capacity))
                .Select(x => new ExportClientsWithMostTrucksDTO()
                    {
                    Name = x.Name,
                    Trucks = x.ClientsTrucks
                    .Where(x => x.Truck.TankCapacity >= capacity)
                    .Select(t => new AllTrucks()
                        {
                        TruckRegistrationNumber = t.Truck.RegistrationNumber,
                        VinNumber = t.Truck.VinNumber,
                        TankCapacity = t.Truck.TankCapacity,
                        CargoCapacity = t.Truck.CargoCapacity,
                        MakeType = t.Truck.MakeType,
                        })
                    .OrderBy(t => t.MakeType)
                    .ThenByDescending(t => t.CargoCapacity)
                    .ToArray()
                    })
                .OrderByDescending(x => x.Trucks.Count())
                .ThenBy(x => x.Name)
                .Take(10)
                .ToArray();

            return result.SerializeToJson().ToString();
            }
        }
    }
