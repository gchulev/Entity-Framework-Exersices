namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Boardgames.Utilities;

    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            ImportCreatorDto[] creatorsDto = XmlHelper.Deserialize<ImportCreatorDto[]>(xmlString, "Creators");

            var sb = new StringBuilder();
            var validCreators = new List<Creator>();

            foreach (ImportCreatorDto creatorDto in creatorsDto)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator validCreator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName
                };

                foreach (ImportBoardgameDto boardgameDto in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgameDto) || string.IsNullOrWhiteSpace(boardgameDto.CategoryType))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame validBoardgame = new Boardgame()
                    {
                        Name = boardgameDto.Name,
                        Rating = boardgameDto.Rating,
                        YearPublished = boardgameDto.YearPublished,
                        CategoryType = (CategoryType)Enum.Parse(typeof(CategoryType), boardgameDto.CategoryType),
                        Mechanics = boardgameDto.Mechanics
                    };

                    validCreator.Boardgames.Add(validBoardgame);
                }

                validCreators.Add(validCreator);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, validCreator.FirstName, validCreator.LastName, validCreator.Boardgames.Count));
            }

            int gamesCount = validCreators.SelectMany(c => c.Boardgames).Count();
            context.Creators.AddRange(validCreators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            ImportSellerDto[] sellersDto = JsonConvert.DeserializeObject<ImportSellerDto[]>(jsonString)!;

            var sb = new StringBuilder();
            var validSellers = new List<Seller>();

            foreach (ImportSellerDto sellerDto in sellersDto)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validSeller = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website
                };

                foreach (int boardGameId in sellerDto.Boardgames.Distinct()) //check if distinct works properly here!!!
                {
                    if (!context.Boardgames.Any(g => g.Id == boardGameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validSeller.BoardgamesSellers.Add(new BoardgameSeller()
                    {
                        BoardgameId = boardGameId
                    });
                }

                validSellers.Add(validSeller);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, validSeller.Name, validSeller.BoardgamesSellers.Count));
            }

            int boardGamesCount = validSellers.SelectMany(s => s.BoardgamesSellers).Count();

            context.Sellers.AddRange(validSellers);
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
