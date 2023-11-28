namespace Trucks.DataProcessor
    {
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Data;
    using Invoices.Extentions;
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
                if (!IsValid(despatcher))
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
            throw new NotImplementedException();
            }

        private static bool IsValid(object dto)
            {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
            }
        }
    }