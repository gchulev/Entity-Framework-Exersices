namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Utilities;

    using Microsoft.EntityFrameworkCore.Diagnostics;

    using Newtonsoft.Json;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            ImportCoachDto[] coachesDto = XmlHelper.Deserialize<ImportCoachDto[]>(xmlString, "Coaches");

            var sb = new StringBuilder();
            var validCoaches = new List<Coach>();

            foreach (ImportCoachDto coachDto in coachesDto)
            {
                if (!IsValid(coachDto) || string.IsNullOrWhiteSpace(coachDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                validCoaches.Add(coach);

                foreach (ImportFootballerDto footballerDto in coachDto.Footballers)
                {

                    if (!IsValid(footballerDto) || DateTime.ParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = DateTime.ParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ContractEndDate = DateTime.ParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BestSkillType = footballerDto.BestSkillType,
                        PositionType = footballerDto.PositionType
                    };

                    coach.Footballers.Add(footballer);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            ImportTeamDto[] teamsDto = JsonConvert.DeserializeObject<ImportTeamDto[]>(jsonString);

            var sb = new StringBuilder();
            var validTeams = new List<Team>();

            foreach (ImportTeamDto teamDto in teamsDto)
            {
                if (!IsValid(teamDto) || teamDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies,
                };

                validTeams.Add(team);

                foreach (int playerId in teamDto.Footballers!.Distinct())
                {
                    if (!context.Footballers.Any(f => f.Id == playerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer footballer = context.Footballers.Find(playerId)!;
                    var teamFootballer = new TeamFootballer()
                    {
                        Footballer = footballer,
                    };

                    team.TeamsFootballers.Add(teamFootballer);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.Teams.AddRange(validTeams);
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
