using Microsoft.EntityFrameworkCore;

namespace _01
    {
    internal class Program
        {
        static void Main(string[] args)
            {
            ApplicationDBContext db = new ApplicationDBContext();

            var towns = db.Towns.Include(t=>t.Country);

            foreach ( var t in towns )
                {
                Console.WriteLine($"{t.Name} is in {t.Country.Name}");
                }


            //var countries = db.Countries;

            //foreach (var country in countries)
            //    {
            //    Console.WriteLine(country.na);
            //    }
            }
        }
    }