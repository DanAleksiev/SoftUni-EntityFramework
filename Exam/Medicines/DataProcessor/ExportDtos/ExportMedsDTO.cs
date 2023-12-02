namespace Medicines.DataProcessor.ExportDtos
    {
    public class ExportMedsDTO
        {
        public string Name { get; set; }
        public string Price { get; set; }

        public Pharmas Pharmacy { get; set; }
    }

    public class Pharmas
        {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
    }
