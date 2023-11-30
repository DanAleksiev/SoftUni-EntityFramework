namespace Footballers.DataProcessor
    {
    using Data;
    using DataProcessor.ExportDto;
    using Invoices.Extentions;

    public class Serializer
        {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
            {
            var result = context.Coaches
                .Select(t => new ExportTeamsWithMostFootballersDTO
                    {
                    CoachName = t.Name,
                    })
                .Take(5)
                .ToArray();

            return "";
            }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
            {
            var result = context.Teams
                .Where(x=>x.TeamsFootballers
                    .Any(p=> p.Footballer.ContractEndDate >= date))
                .Select(t => new ExportCoachesWithTheirFootballersDTO
                    {
                    Name = t.Name,
                    Foodballers = t.TeamsFootballers
                    .Where(p => p.Footballer.ContractEndDate >= date)
                    .OrderByDescending(f=>f.Footballer.ContractEndDate)
                    .ThenBy(f=>f.Footballer.Name)
                    .Select(f => new AllFoodballers()
                        {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d"),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d"),
                        BestSkillType = f.Footballer.BestSkillType,
                        PositionType = f.Footballer.PositionType,
                        }).ToArray()
                    })
                .OrderByDescending(t=>t.Foodballers.Count())
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return result.SerializeToJson();
            }
        }
    }
