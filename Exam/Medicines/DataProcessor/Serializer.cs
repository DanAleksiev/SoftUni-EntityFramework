namespace Medicines.DataProcessor
    {
    using Invoices.Extentions;
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualBasic;
    using System.Globalization;
    using System.Xml;

    public class Serializer
        {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
            {

            var result = context.Patients
                .Where(p => p.PatientsMedicines
                    .Any(m => m.Medicine.ProductionDate > DateTime.Parse(date)))
              .ToArray()
              .Select(c => new ExportPatientsDTO()
                  {
                  Gender = c.Gender.ToString().ToLower(),
                  Name = c.FullName,
                  AgeGroup = c.AgeGroup.ToString(),
                  Medicines = c.PatientsMedicines
                  .OrderByDescending(c => c.Medicine.ExpiryDate)
                  .ThenBy(c => c.Medicine.Price)
                  .Where(m => m.Medicine.ProductionDate > DateTime.Parse(date))
                  .Select(m => new AllMedicines
                      {
                      Category = m.Medicine.Category.ToString().ToLower(),
                      Name = m.Medicine.Name,
                      Price = m.Medicine.Price.ToString("f2"),
                      Producer = m.Medicine.Producer,
                      BestBefore = m.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                      })

                  .ToArray()
                  })
              .OrderByDescending(c => c.Medicines.Count())
              .ThenBy(c => c.Name)
              .ToArray();


            XmlFormating xmlFormater = new XmlFormating();

            return xmlFormater.Serialize<ExportPatientsDTO>(result, "Patients");
            }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
            {
            var result = context.Medicines
                .Include(c => c.Pharmacy)
                .Where(c => c.Pharmacy.IsNonStop == true && (Category)medicineCategory == c.Category)
                .Select(c => new ExportMedsDTO
                    {
                    Name = c.Name,
                    Price = c.Price.ToString("f2"),
                    Pharmacy = new Pharmas
                        {
                        Name = c.Pharmacy.Name,
                        PhoneNumber = c.Pharmacy.PhoneNumber,
                        }
                    })
                .OrderBy(c=>c.Price)
                .ThenBy(c=>c.Name)
                .ToArray();

            return result.SerializeToJson();
            }
        }
    }
