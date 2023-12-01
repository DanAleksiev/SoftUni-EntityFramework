namespace Footballers.DataProcessor
    {
    using Data;
    using DataProcessor.ExportDto;
    using Invoices.Extentions;
    using System.Globalization;

    public class Serializer
        {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
            {
            var result = context.Coaches
                .Where(c => c.Footballers.Any())
                .ToArray()
                .Select(t => new ExportCoachesWithTheirFootballersDTO
                    {
                    Count = t.Footballers.Count(),
                    CoachName = t.Name,
                    Footballers = t.Footballers.Select(f => new AllPlayers()
                        {
                        Name = f.Name,
                        Position = f.PositionType.ToString(),
                        })
                    .OrderBy(p => p.Name)
                    .ToArray(),
                    })
                .OrderByDescending(c=> c.Count)
                .ToArray();

            XmlFormating format = new XmlFormating();
            return format.Serialize<ExportCoachesWithTheirFootballersDTO[]>(result, "Coaches");
            }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
            {
            var result = context.Teams
                .Where(x=>x.TeamsFootballers
                    .Any(p=> p.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                    {
                    Name = t.Name,
                    Foodballers = t.TeamsFootballers
                    .Where(p => p.Footballer.ContractStartDate >= date)
                    .OrderByDescending(f=>f.Footballer.ContractEndDate)
                    .ThenBy(f=>f.Footballer.Name)
                    .Select(f => new 
                        {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType,
                        PositionType = f.Footballer.PositionType,
                        })
                    .ToArray()
                    })
                .OrderByDescending(t=>t.Foodballers.Length)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return result.SerializeToJson();
            }
        }
    }
