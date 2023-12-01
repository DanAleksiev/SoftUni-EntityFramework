namespace Footballers.DataProcessor
    {
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Invoices.Extentions;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
        {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
            {
            XmlFormating format = new XmlFormating();
            ImportCoachesDTO[] dto = format.Deserialize<ImportCoachesDTO[]>(xmlString, "Coaches");

            StringBuilder sb = new StringBuilder();
            List<Coach> coachList = new List<Coach>();

            foreach (var coach in dto)
                {
                if (!IsValid(coach)||string.IsNullOrEmpty(coach.Nationality))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Coach currentCoach = new Coach()
                    {
                    Name = coach.Name,
                    Nationality = coach.Nationality,
                    };

                foreach (var member in coach.Footballers)
                    {
                    DateTime startDate;
                    DateTime endDate;

                    if (!IsValid(member) || !DateTime.TryParseExact(member.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate)
                        || !DateTime.TryParseExact(member.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    

                    if (endDate < startDate)
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    Footballer currentFootballer = new Footballer()
                        {
                        Name = member.Name,
                        ContractStartDate = startDate,
                        ContractEndDate = endDate,
                        BestSkillType = (BestSkillType)member.BestSkillType,
                        PositionType = (PositionType)member.PositionType,
                        Coach = currentCoach,
                        };

                    currentCoach.Footballers.Add(currentFootballer);
                    }
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, currentCoach.Name, currentCoach.Footballers.Count));
                coachList.Add(currentCoach);
                }

            context.Coaches.AddRange(coachList);
            context.SaveChanges();
            return sb.ToString().Trim();
            }

        public static string ImportTeams(FootballersContext context, string jsonString)
            {
            ImportTeamsDTO[] dto = jsonString.DeserializeFromJson<ImportTeamsDTO[]>();

            StringBuilder sb = new StringBuilder();
            List<Team> teams = new();
            var players = context.Footballers.Select(x => x.Id).ToArray();

            foreach (var team in dto)
                {
                if (!IsValid(team) || team.Trophies == "0" || String.IsNullOrEmpty(team.Nationality))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Team currentTeam = new Team()
                    {
                    Name = team.Name,
                    Nationality = team.Nationality,
                    Trophies = int.Parse(team.Trophies)
                    };

                foreach (var player in team.Footballers.Distinct())
                    {
                    if (!players.Contains(player))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        };

                    currentTeam.TeamsFootballers.Add(new TeamFootballer
                        {
                        TeamId = currentTeam.Id,
                        FootballerId = player
                        });
                    }

                sb.AppendLine(string.Format(SuccessfullyImportedTeam, currentTeam.Name, currentTeam.TeamsFootballers.Count()));
                teams.Add(currentTeam);
                }

            context.Teams.AddRange(teams);
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
