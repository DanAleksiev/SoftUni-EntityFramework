using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ExportDto
    {
    public class ExportSellersDTO
        {
        public string Name { get; set; }
        public string Website { get; set; }

        public AllBoardgame[] Boardgames { get; set; }
    }

    public class AllBoardgame
        {
        public string Name { get; set; }
        public double Rating { get; set; }
        public string Mechanics { get; set; }

        [EnumDataType(typeof(CategoryType))]
        public CategoryType Category { get; set; }
    }
    }
