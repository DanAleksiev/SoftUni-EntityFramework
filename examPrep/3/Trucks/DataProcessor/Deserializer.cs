namespace Trucks.DataProcessor
    {
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Text;
    using Data;
    using Invoices.Extentions;
    using Microsoft.EntityFrameworkCore;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
        {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
            {
            XmlFormating formating = new XmlFormating();
            StringBuilder sb = new StringBuilder();
            List<Despatcher> validDespachers = new List<Despatcher>();

            ImportDespatcherDTO[] dto = formating.Deserialize<ImportDespatcherDTO[]>(xmlString, "Despatchers");

            foreach (var despatcher in dto)
                {
                if (!IsValid(despatcher)|| String.IsNullOrEmpty(despatcher.Position))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Despatcher currentDespatcher = new Despatcher()
                    {
                    Name = despatcher.Name,
                    Position = despatcher.Position,
                    };

                foreach (var truck in despatcher.Trucks)
                {
                    if (!IsValid(truck))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    currentDespatcher.Trucks.Add(new Truck
                        {
                        RegistrationNumber = truck.RegistrationNumber,
                        VinNumber = truck.VinNumber,
                        TankCapacity = truck.TankCapacity,
                        CargoCapacity = truck.CargoCapacity,
                        CategoryType = (CategoryType)truck.CategoryType,
                        MakeType = (MakeType)truck.MakeType,
                        });

                }
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, currentDespatcher.Name, currentDespatcher.Trucks.Count));
                validDespachers.Add(currentDespatcher);
            };

            context.Despatchers.AddRange(validDespachers);
            context.SaveChanges();
            return sb.ToString().Trim();
            }
        public static string ImportClient(TrucksContext context, string jsonString)
            {
            StringBuilder sb = new StringBuilder();

            ImportClientDTO[] dto = jsonString.DeserializeFromJson<ImportClientDTO[]>();
            var trucks = context.Trucks.Select(x => x.Id).ToList();

            List<Client> validClients = new List<Client>();

            foreach (var client in dto)
                {
                if (!IsValid(client)|| client.Type == "usual")
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Client currentClient = new Client()
                    {
                    Name= client.Name,
                    Nationality= client.Nationality,
                    Type = client.Type,
                    };

                foreach (var truck in client.Trucks.Distinct())
                    {
                    if (!trucks.Contains(truck))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    currentClient.ClientsTrucks.Add(new ClientTruck
                        {
                        TruckId = truck
                        });
                    }
                sb.AppendLine(string.Format(SuccessfullyImportedClient, currentClient.Name, currentClient.ClientsTrucks.Count()));
                validClients.Add(currentClient);
                };

            context.Clients.AddRange(validClients);
            context.SaveChanges();
            return sb.ToString().Trim();
            }

        private static bool IsValid(object dto)
            {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
            }
        }
    }