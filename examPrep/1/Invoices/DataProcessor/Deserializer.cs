namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using ProductShop;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";

        public static IMapper CreateMapper()
            {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<InvoicesProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
            }

        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var serialiser = new XmlFormating();
            var mapper = CreateMapper();

            ImportClientsDTO[] dto = serialiser.Deserialize<ImportClientsDTO[]>(xmlString, "Clients");
            StringBuilder sb = new StringBuilder();
            foreach (var client in dto)
            {
                try
                    {
                    var newClient = mapper.Map<Client>(client);
                    context.Clients.Add(newClient);
                    sb.AppendLine($"Successfully imported client {newClient.Name}");
                    }
                catch
                    {
                    sb.AppendLine("Invalid data!");
                    
                    }
                
            }
            //context.SaveChanges();
            return sb.ToString().Trim();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {


            throw new NotImplementedException();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
