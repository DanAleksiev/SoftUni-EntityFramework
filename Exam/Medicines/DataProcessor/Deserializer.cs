namespace Medicines.DataProcessor
{
    using AutoMapper.Execution;
    using Invoices.Extentions;
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
            {
            List<ImportPatientsDTO> dto = jsonString.DeserializeFromJson<List<ImportPatientsDTO>>();
            StringBuilder sb = new StringBuilder();

            List<Patient> products = new List<Patient>();

            var clients = context.Medicines.Select(p => p.Id).ToList();

            foreach (var patient in dto)
                {
                if (!IsValid(patient))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }


                var currPatient = new Patient
                    {
                    FullName = patient.FullName,
                    AgeGroup = patient.AgeGroup,
                    Gender = patient.Gender,
                    };

                List<int> addedMeds = new();
                foreach (var med in patient.Medicines)
                    {
                    if (!clients.Contains(med) || addedMeds.Contains(med))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }
                    currPatient.PatientsMedicines.Add(new PatientMedicine()
                        {
                        MedicineId = med,
                        Patient = currPatient
                        });
                    addedMeds.Add(med);
                    }

                sb.AppendLine(string.Format(SuccessfullyImportedPatient, currPatient.FullName, currPatient.PatientsMedicines.Count));
                products.Add(currPatient);
                }

            context.Patients.AddRange(products);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
            }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            var serialiser = new XmlFormating();

            ImportPharmaciesDTO[] dto = serialiser.Deserialize<ImportPharmaciesDTO[]>(xmlString, "Pharmacies");
            StringBuilder sb = new StringBuilder();

            List<Pharmacy> validP = new List<Pharmacy>();
            foreach (var pharma in dto)
                {
                var validBool = new[]{ "true", "false" };
                if (!IsValid(pharma) || !validBool.Contains(pharma.IsNonStop))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }


                var newPharma = new Pharmacy
                    {
                    Name = pharma.Name,
                    PhoneNumber = pharma.PhoneNumber,
                    IsNonStop = bool.Parse(pharma.IsNonStop),
                    };
                Dictionary<string, string> medsInPharma = new();

                foreach (var med in pharma.Medicines.Distinct())
                    {
                    
                    DateTime productionDate;
                    DateTime expiryDate;

                    if (!DateTime.TryParseExact(med.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out productionDate)
                        || !DateTime.TryParseExact(med.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out expiryDate))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    if (!IsValid(med)||productionDate >= expiryDate )
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    if(medsInPharma.ContainsKey(med.Name) && medsInPharma[med.Name] == med.Producer)
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    newPharma.Medicines.Add(new Medicine()
                        {
                        Name = med.Name,
                        Price = med.Price,
                        Category = (Category)med.Category,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = med.Producer,
                        Pharmacy = newPharma
                        });
                    medsInPharma[med.Name] = med.Producer;
                    }

                    validP.Add(newPharma);
                    sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, newPharma.Name, newPharma.Medicines.Count));
                }
            context.Pharmacies.AddRange(validP);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
            }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
