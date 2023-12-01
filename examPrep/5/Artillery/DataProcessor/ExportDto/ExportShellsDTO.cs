using Artillery.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Artillery.DataProcessor.ExportDto
    {
    public class ExportShellsDTO
        {
        public double ShellWeight { get; set; }

        public string Caliber { get; set; }

        public AllGuns[] Guns { get; set; }
    }

    public class AllGuns
        {
        [EnumDataType(typeof(GunType))]
        public GunType GunType { get; set; }

        public int GunWeight { get; set; }
        public double BarrelLength { get; set; }
        public string Range { get; set; }
    }
    }
