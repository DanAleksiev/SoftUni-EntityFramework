namespace Boardgames.DataProcessor
    {
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Invoices.Extentions;
    using Microsoft.Data.SqlClient.Server;

    public class Deserializer
        {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
            {
            XmlFormating format = new XmlFormating();
            ImportCreatorsDTO[] dto = format.Deserialize<ImportCreatorsDTO[]>(xmlString, "Creators");
            StringBuilder sb = new StringBuilder();
            List<Creator> creators = new List<Creator>();

            foreach (var creator in dto)
                {
                if (!IsValid(creator))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Creator currentCreator = new Creator()
                    {
                    FirstName = creator.FirstName,
                    LastName = creator.LastName,
                    };

                foreach (var bg in creator.Boardgames)
                    {
                    if (!IsValid(bg))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }
                    Boardgame newBoardGame = new Boardgame()
                        {
                        Name = bg.Name,
                        Rating = bg.Rating,
                        YearPublished = bg.YearPublished,
                        CategoryType = (CategoryType)bg.CategoryType,
                        Mechanics = bg.Mechanics,
                        Creator = currentCreator
                        };
                    context.Boardgames.Add(newBoardGame);
                    currentCreator.Boardgames.Add(newBoardGame);


                    }

                string count = currentCreator.Boardgames.Count().ToString();
                creators.Add(currentCreator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, currentCreator.FirstName, currentCreator.LastName , count));
                }

            context.Creators.AddRange(creators);
            context.SaveChanges();
            return sb.ToString().Trim();
            }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
            {
            List<ImportSellerDTO> dto = jsonString.DeserializeFromJson<List<ImportSellerDTO>>() ;
            StringBuilder sb = new StringBuilder();
            List<Seller> sellers = new List<Seller>();

            var boardgames = context.Boardgames.Select(b => b.Id).ToArray();

            foreach (var seller in dto)
                {
                if (!IsValid(seller))
                    {
                    sb.AppendLine(ErrorMessage);
                    continue;
                    }

                Seller currentSeller = new ()
                    {
                    Name = seller.Name,
                    Address = seller.Address,
                    Country = seller.Country,
                    Website = seller.Website,
                    };

                foreach (var bg in seller.Boardgames.Distinct())
                    {
                    if (!boardgames.Contains(bg))
                        {
                        sb.AppendLine(ErrorMessage);
                        continue;
                        }

                    currentSeller.BoardgamesSellers.Add(new BoardgameSeller
                        {
                        BoardgameId = bg
                        });
                    }

                sellers.Add(currentSeller);
                sb.AppendLine(
                    string.Format(SuccessfullyImportedSeller, currentSeller.Name , currentSeller.BoardgamesSellers.Count()));
                }
            

            context.Sellers.AddRange(sellers);
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
