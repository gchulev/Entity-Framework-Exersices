namespace VaporStore.DataProcessor
{
    using Data;

    using Newtonsoft.Json;

    using VaporStore.DataProcessor.ExportDto;
    using Data.Models.Enums;
    using System.Globalization;
    using VaporStore.Utilities;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var gamesExport = context.Genres
                .ToArray()
                .Where(gn => genreNames.Contains(gn.Name))
                .Select(gn => new
                {
                    Id = gn.Id,
                    Genre = gn.Name,
                    Games = gn.Games
                        .Where(g => g.Purchases.Any())
                        .Select(g => new
                        {
                            Id = g.Id,
                            Title = g.Name,
                            Developer = g.Developer.Name,
                            Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
                            Players = g.Purchases.Count
                        })
                        .OrderByDescending(gm => gm.Players)
                        .ThenBy(gm => gm.Id)
                        .ToArray(),
                    TotalPlayers = gn.Games.Sum(g => g.Purchases.Count)
                })
                .OrderByDescending(gn => gn.TotalPlayers)
                .ThenBy(gn => gn.Id)
                .ToArray();


            string jsonResult = JsonConvert.SerializeObject(gamesExport, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            ExportUserDto[] usersDto = context.Purchases
                .ToArray()
                .Where(p => p.Type.ToString() == purchaseType && p.Card.User != null) // filter by purchase type and exclude null users
                .GroupBy(p => p.Card.UserId) // group by user id
                .Select(g => new ExportUserDto()
                {
                    Username = g.First().Card.User.Username,
                    Purchases = g.Select(p => new ExportPurchaseDto()
                    {
                        Card = p.Card.Number,
                        Cvc = p.Card.Cvc,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new ExportGameDto()
                        {
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price,
                            Title = p.Game.Name
                        },
                    })
                    .OrderBy(p => p.Date)
                    .ToArray(),
                    TotalSpent = g.Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Any()) // exclude users with no purchases
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(usersDto, "Users");

            return xmlResult;
        }
    }
}