
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Invoices.Extentions;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {

            GunType targetGuns = Enum.Parse<GunType>("AntiAircraftGun");

            var result = context.Shells
                .Where(s=>s.ShellWeight >shellWeight)
                .Select(s => new ExportShellsDTO
                    {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                    .Where(g=>g.GunType == targetGuns)
                    .Select(g=> new AllGuns()
                        {
                        GunType = g.GunType,
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range >= 3000 ? "Long-range" : "Regular range"
                        })
                    .OrderByDescending(g => g.GunWeight)
                    .ToArray()
                    })
                .OrderBy(s=>s.ShellWeight)
                .ToArray();

            return result.SerializeToJson();
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var result = context.Guns
                .Where(g=>g.Manufacturer.ManufacturerName == manufacturer)
                .Select(g=> new ExportGunsDTO
                    {
                    Manufacturer = manufacturer,
                    GunType = g.GunType.ToString(),
                    GunWeight= g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    Range = g.Range,
                    Countries = g.CountriesGuns
                        .Where(g=>g.Country.ArmySize > 4500000)
                        .Select(c=> new AllGunCountries (){
                        Country = c.Country.CountryName,
                        ArmySize = c.Country.ArmySize
                        })
                        .OrderBy(c=>c.ArmySize)
                        .ToArray()
                    })
                .OrderBy(c=>c.BarrelLength)
                .ToArray();

            XmlFormating format = new XmlFormating();

            return format.Serialize<ExportGunsDTO[]>(result, "Guns");

        }
    }
}
