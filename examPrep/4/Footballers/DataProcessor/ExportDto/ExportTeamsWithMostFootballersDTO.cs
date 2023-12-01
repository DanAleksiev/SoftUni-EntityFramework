using Footballers.Data.Models.Enums;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
    {

    public class ExportTeamsWithMostFootballersDTO

        {
        public string Name { get; set; }

        public AllFoodballers[] Footballers { get; set; }
        }

    public class AllFoodballers
        {
        public string FootballerName { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public BestSkillType BestSkillType { get; set; }
        public PositionType PositionType { get; set; }
        }
    }
