namespace Footballers.DataProcessor
{
    using System.Globalization;

    using Data;

    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.Utilities;

    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            ExportCoachDto[] coaches = context.Coaches
                .AsNoTracking()
                .Where(c => c.Footballers.Count > 0)
                .Select(c => new ExportCoachDto()
                {
                    CoachName = c.Name,
                    Footballers = c.Footballers.Select(f => new ExportShortFootballerDto()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(f => f.Name)
                    .ToArray()
                        ,
                    FootballersCount = c.Footballers.Count
                })
                .OrderByDescending(c => c.FootballersCount)
                .ThenBy(c => c.CoachName)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(coaches, "Coaches");

            return xmlResult;
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            //DateTime convertedDate = DateTime.ParseExact(date.ToShortDateString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //ExportTeamDto[] teamsDto = context.Teams
            //    .AsNoTracking()
            //    .Include(t => t.TeamsFootballers)
            //    .ThenInclude(tf => tf.Footballer)
            //    .ToArray()
            //    .Where(t => t.TeamsFootballers.Any(tf => DateTime.ParseExact(tf.Footballer.ContractStartDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), "MM/dd/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture)))
            //    .Select(t => new ExportTeamDto()
            //    {
            //        Name = t.Name,
            //        Footballers = t.TeamsFootballers.Where(tf => DateTime.ParseExact(tf.Footballer.ContractStartDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture))
            //            .Select(t => new ExportFootballerDto()
            //            {
            //                FootballerName = t.Footballer.Name,
            //                ContractStartDate = DateTime.ParseExact(t.Footballer.ContractStartDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"),
            //                ContractEndDate = DateTime.ParseExact(t.Footballer.ContractEndDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"),
            //                PositionType = t.Footballer.PositionType.ToString(),
            //                BestSkillType = t.Footballer.BestSkillType.ToString()
            //            })
            //            .OrderByDescending(f => DateTime.ParseExact(f.ContractEndDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture))
            //            .ThenBy(f => f.FootballerName)
            //            .ToArray()
            //    })
            //    .OrderByDescending(t => t.Footballers.Where(f => DateTime.ParseExact(f.ContractStartDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture) >= date).Count())
            //    .ThenBy(t => t.Name)
            //    .Take(5)
            //    .ToArray();

            DateTime convertedDate = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Team[] teams = context.Teams
                .AsNoTracking()
                .AsNoTracking()
                .Include(t => t.TeamsFootballers)
                .ThenInclude(tf => tf.Footballer)
                .ToArray();

            var validTeam = new List<ExportTeamDto>();

            foreach (Team team in teams)
            {
                if (team.TeamsFootballers.Any(f => DateTime.ParseExact(f.Footballer.ContractStartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture) >= convertedDate))
                {
                    validTeam.Add(new ExportTeamDto()
                    {
                        Name = team.Name,
                        Footballers = team.TeamsFootballers.Where(tf => DateTime.ParseExact(tf.Footballer.ContractStartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture) >= convertedDate).Select(tf => new ExportFootballerDto()
                        {
                            FootballerName = tf.Footballer.Name,
                            ContractStartDate = DateTime.ParseExact(tf.Footballer.ContractStartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            ContractEndDate = DateTime.ParseExact(tf.Footballer.ContractEndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            PositionType = tf.Footballer.PositionType.ToString(),
                            BestSkillType = tf.Footballer.BestSkillType.ToString()
                        })
                        .OrderByDescending(f => f.ContractEndDate)
                        .ThenBy(f => f.FootballerName)
                        .ToArray()
                    });
                }
            }

            var orderedTeams = validTeam
                .OrderByDescending(t => t.Footballers.Length)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            var serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateFormatString = "MM/dd/yyyy",
                Culture = CultureInfo.InvariantCulture
            };

            string jsonResult = JsonConvert.SerializeObject(orderedTeams, serializerSettings);

            return jsonResult;
        }
    }
}
