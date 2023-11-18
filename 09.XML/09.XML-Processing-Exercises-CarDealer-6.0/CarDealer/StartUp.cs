using AutoMapper;
using CarDealer.Data;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {

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
        }
}