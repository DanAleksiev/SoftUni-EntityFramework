﻿namespace Footballers.DataProcessor
    {
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Invoices.Extentions;
    using System.ComponentModel.DataAnnotations;
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
                if (!IsValid(coach))
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


                    if (!IsValid(member))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    var startDate = DateTime.Parse(member.ContractStartDate);
                    var endDate = DateTime.Parse(member.ContractEndDate);

                    if(endDate < startDate)
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
                sb.AppendLine(string.Format(SuccessfullyImportedCoach,currentCoach.Name,currentCoach.Footballers.Count));
                coachList.Add(currentCoach);
                }

            context.Coaches.AddRange(coachList);
            context.SaveChanges();
            return sb.ToString().Trim();
            }

        public static string ImportTeams(FootballersContext context, string jsonString)
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
