namespace Artillery.DataProcessor
    {
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ImportDto;
    using Invoices.Extentions;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public class Deserializer
        {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
            {
            XmlFormating formating = new XmlFormating();
            ImportCountriesDTO[] dto = formating.Deserialize<ImportCountriesDTO[]>(xmlString, "Countries");

            List<Country> validC = new List<Country>();
            StringBuilder sb = new StringBuilder();

            foreach (var country in dto)
                {
                if (!IsValid(country))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Country currCountry = new Country()
                    {
                    CountryName = country.CountryName,
                    ArmySize = country.ArmySize,
                    };

                validC.Add(currCountry);
                sb.AppendLine(string.Format(SuccessfulImportCountry, currCountry.CountryName, currCountry.ArmySize));
                }

            context.Countries.AddRange(validC);
            context.SaveChanges();
            return sb.ToString();
            }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
            {
            XmlFormating formating = new XmlFormating();
            ImportManufacturersDTO[] dto = formating.Deserialize<ImportManufacturersDTO[]>(xmlString, "Manufacturers");

            List<Manufacturer> validM = new List<Manufacturer>();
            StringBuilder sb = new StringBuilder();
            List<string> names = new List<string>();
            foreach (var manu in dto)
                {
                if (!IsValid(manu))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Manufacturer currManu = new Manufacturer()
                    {
                    ManufacturerName = manu.ManufacturerName,
                    Founded = manu.Founded,
                    };


                if (names.Contains(currManu.ManufacturerName))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                string countryName = currManu.Founded.Split(", ").LastOrDefault().ToString();
                names.Add(currManu.ManufacturerName);
                validM.Add(currManu);
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, currManu.ManufacturerName,countryName));
                }

            context.Manufacturers.AddRange(validM);
            context.SaveChanges();
            return sb.ToString();
            }

        public static string ImportShells(ArtilleryContext context, string xmlString)
            {
            XmlFormating formating = new XmlFormating();
            ImportShellsDTO[] dto = formating.Deserialize<ImportShellsDTO[]>(xmlString, "Shells");

            List<Shell> validS = new List<Shell>();
            StringBuilder sb = new StringBuilder();
            foreach (var shell in dto)
                {
                if (!IsValid(shell))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Shell currShell = new Shell()
                    {
                    ShellWeight = shell.ShellWeight,
                    Caliber = shell.Caliber,
                    };

                validS.Add(currShell);
                sb.AppendLine(string.Format(SuccessfulImportShell, currShell.Caliber, currShell.ShellWeight));
                }

            context.Shells.AddRange(validS);
            context.SaveChanges();
            return sb.ToString();
            }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
            {
            throw new NotImplementedException();
            }
        private static bool IsValid(object obj)
            {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
            }
        }
    }