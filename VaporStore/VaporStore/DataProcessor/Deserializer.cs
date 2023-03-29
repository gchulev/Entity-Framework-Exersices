namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDto;
    using VaporStore.Utilities;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            ImportGameDto[] gamesDto = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString)!;

            var sb = new StringBuilder();
            var validGames = new List<Game>();
            var validDevelopers = new List<Developer>();
            var validGenre = new List<Genre>();
            var validTags = new List<Tag>();

            foreach (ImportGameDto gameDto in gamesDto)
            {
                if (!IsValid(gameDto)
                    || string.IsNullOrWhiteSpace(gameDto.Name)
                    || string.IsNullOrWhiteSpace(gameDto.Developer)
                    || string.IsNullOrWhiteSpace(gameDto.Genre)
                    || gameDto.Tags == null
                    || gameDto.Tags.Length == 0
                    )
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!validDevelopers.Any(d => d.Name == gameDto.Developer))
                {
                    var dev = new Developer()
                    {
                        Name = gameDto.Developer
                    };
                    validDevelopers.Add(dev);
                }
                var validDev = validDevelopers.FirstOrDefault(d => d.Name == gameDto.Developer);

                if (!validGenre.Any(g => g.Name == gameDto.Genre))
                {
                    var genre = new Genre()
                    {
                        Name = gameDto.Genre
                    };
                    validGenre.Add(genre);
                }
                var gn = validGenre.FirstOrDefault(g => g.Name == gameDto.Genre);

                var CurrentValidTags = new List<Tag>();

                if (gameDto.Tags != null && gameDto.Tags.Length > 0)
                {
                    foreach (string tagName in gameDto.Tags)
                    {
                        if (!validTags.Any(t => t.Name == tagName))
                        {
                            var validTag = new Tag()
                            {
                                Name = tagName
                            };

                            validTags.Add(validTag);
                        }

                        var currentValidTag = validTags.FirstOrDefault(t => t.Name == tagName);
                        CurrentValidTags.Add(currentValidTag!);
                    }
                }

                var game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = validDev!,
                    Genre = gn!,
                    GameTags = CurrentValidTags.Select(t => new GameTag()
                    {
                        Tag = t
                    })
                    .ToArray()
                };

                validGames.Add(game);
                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, game.Genre.Name, game.GameTags.Count));
            }

            context.Games.AddRange(validGames);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            ImportUserDto[] usersDto = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString)!;

            var sb = new StringBuilder();
            var validUsers = new List<User>();
            var validCards = new List<Card>();

            foreach (ImportUserDto userDto in usersDto)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validUser = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (ImportCardDto cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var validCard = new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = cardDto.Type
                    };

                    validUser.Cards.Add(validCard);
                }

                validUsers.Add(validUser);
                sb.AppendLine(string.Format(SuccessfullyImportedUser, validUser.Username, validUser.Cards.Count));
            }

            int cardsCount = validUsers.SelectMany(vu => vu.Cards).Count();

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            ImportPurchaseDto[] purchasesDto = XmlHelper.Deserialize<ImportPurchaseDto[]>(xmlString, "Purchases");

            var sb = new StringBuilder();
            var validPurchases = new List<Purchase>();

            foreach (ImportPurchaseDto ImportPurchaseDto in purchasesDto)
            {
                if (!IsValid(ImportPurchaseDto) || string.IsNullOrWhiteSpace(ImportPurchaseDto.Date))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Card[] allCards = context.Cards.ToArray();
                Card purchaseCard = allCards.FirstOrDefault(c => c.Number == ImportPurchaseDto.Card);

                if (purchaseCard == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game[] allGames = context.Games.ToArray();
                Game currentGame = context.Games.FirstOrDefault(g => g.Name == ImportPurchaseDto.Title);

                if (currentGame == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validPurchase = new Purchase()
                {
                    Type = ImportPurchaseDto.Type,
                    ProductKey = ImportPurchaseDto.Key,
                    Card = purchaseCard,
                    Game = currentGame,
                    Date = DateTime.ParseExact(ImportPurchaseDto.Date.ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    
                };

                User currentUser = context.Users.FirstOrDefault(u => u.Cards.Any(c => c.Number == ImportPurchaseDto.Card));

                validPurchases.Add(validPurchase);
                sb.AppendLine(string.Format(SuccessfullyImportedPurchase, validPurchase.Game.Name, currentUser.Username));
            }

            context.Purchases.AddRange(validPurchases);
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