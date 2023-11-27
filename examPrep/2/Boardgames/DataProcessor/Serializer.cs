namespace Boardgames.DataProcessor
    {
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Invoices.Extentions;

    public class Serializer
        {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
            {
            return "";
            }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
            {
            var result = context.Sellers
                .Where(s => s.BoardgamesSellers
                    .Any(bs => bs.Boardgame.YearPublished >= year
                        && bs.Boardgame.Rating <= rating))
                .Select(s => new ExportSellersDTO
                    {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where((bs => bs.Boardgame.YearPublished >= year
                        && bs.Boardgame.Rating <= rating)).Select(bs => new AllBoardgame
                        {
                        Name = bs.Boardgame.Name,
                        Rating = bs.Boardgame.Rating,
                        Mechanics = bs.Boardgame.Mechanics,
                        Category = bs.Boardgame.CategoryType
                        })
                    .OrderByDescending(s => s.Rating)
                    .ThenBy(s => s.Name)
                    .ToArray()
                    })
                .OrderByDescending(s => s.Boardgames.Count())
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            return result.SerializeToJson();
            }
        }
    }