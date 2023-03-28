namespace Theatre.DataProcessor
{

    using System;

    using Newtonsoft.Json;

    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.Utilities;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            ExportTheatreDto[] theatresDto = context.Theatres
                .Where(th => th.NumberOfHalls >= numbersOfHalls && th.Tickets.Count >= 20)
                .Select(th => new ExportTheatreDto()
                {
                    Name = th.Name,
                    Halls = th.NumberOfHalls,
                    TotalIncome = th.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Select(t => t.Price).Sum(),
                    Tickets = th.Tickets.Where(t => t.RowNumber >= 1 && t.RowNumber <= 5).Select(t => new ExportTicketDto()
                    {
                        Price = t.Price,
                        RowNumber = t.RowNumber 
                    })
                    .OrderByDescending(t => t.Price)
                    .ToArray()
                })
                .OrderByDescending(th => th.Halls)
                .ThenBy(th => th.Name)
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(theatresDto, Formatting.Indented);

            return jsonResult;
        }

        public static string ExportPlays(TheatreContext context, double raiting)
        {
            ExportPlayDto[] playsDto = context.Plays
                .Where(p => p.Rating <= raiting)
                .Select(p => new ExportPlayDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre,
                    Actors = p.Casts.Where(c => c.IsMainCharacter).Select(c => new ExportActorDto()
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    })
                    .OrderByDescending(a => a.FullName)
                    .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            string xmlResult = XmlHelper.Serialize(playsDto, "Plays");

            return xmlResult;
        }
    }
}
