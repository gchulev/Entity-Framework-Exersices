namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;

    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            ExportCreatorDto[] creatorsDto = context.Creators
                .ToArray()
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorDto()
                {
                    BoardgamesCount = c.Boardgames.Count,
                    CreatorName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames.Select(bg => new ExportBoardgameDto()
                    {
                        Name = bg.Name,
                        YearPublished = bg.YearPublished
                    })
                    .OrderBy(bg => bg.Name)
                    .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(creatorsDto, "Creators");
            return xmlResult;
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(g => g.Boardgame.YearPublished >= year && g.Boardgame.Rating <= rating))
                .Select(s => new
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers.Where(bg => bg.Boardgame.YearPublished >= year && bg.Boardgame.Rating <= rating).Select(g => new 
                    { 
                        Name = g.Boardgame.Name,
                        Rating = g.Boardgame.Rating,
                        Mechanics = g.Boardgame.Mechanics,
                        Category = g.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(g => g.Rating)
                    .ThenBy(g => g.Name)
                    .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            var jsonResult = JsonConvert.SerializeObject(sellers, Formatting.Indented);

            return jsonResult;
        }
    }
}