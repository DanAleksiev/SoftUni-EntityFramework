using AutoMapper;
using Invoices.Extentions;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace CheetSheet
    {
    internal class Program
        {
        
        //ConnectionState: "Server=MSI\\SQLEXPRESS;Database=Exam;Integrated Security=True;MultipleActiveResultSets=true;Encrypt=False";
        static void Main(string[] args)
            {
            //CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            //read file
            //CarDealerContext context = new CarDealerContext();
            string salesJson = File.ReadAllText("../../../Datasets/sales.json");

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //Json import
            //...DTO[] import = JsonConvert.DeserializeObject<...DTO[]>(inputJson);
            //ICollection<> carsToAdd = new HashSet<>();

            //context.---.AddRange(---);
            //context.SaveChanges();

            //json export
            //var json = JsonConvert.SerializeObject(result, Formatting.Indented);
            }

        //mapper
        public static IMapper CreateMapper()
            {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<--->();
            });



            IMapper mapper = configuration.CreateMapper();

            return mapper;
            }

        //xml serialise or use xml formating
        private static string SerializeToXml<T>(T dto, string xmlRootAttribute)
            {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));

            StringBuilder stringBuilder = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
                {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                    {
                    xmlSerializer.Serialize(stringWriter, dto, xmlSerializerNamespaces);
                    }
                catch (Exception)
                    {

                    throw;
                    }
                }

            return stringBuilder.ToString();
            }



        }
    }