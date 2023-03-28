namespace Theatre.DataProcessor
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net;
    using System.Text;

    using Newtonsoft.Json;

    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;
    using Theatre.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            ImportPlayDto[] playsDto = XmlHelper.Deserialize<ImportPlayDto[]>(xmlString, "Plays");

            var sb = new StringBuilder();
            var validPlays = new List<Play>();

            foreach (ImportPlayDto playDto in playsDto)
            {
                Tuple<TimeSpan,bool> isValidDuration = ConvertAndValidateStringToTimespan(playDto.Duration);
                bool isValid = isValidDuration.Item2;

                if (!isValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration = isValidDuration.Item1;

                if (!IsValid(playDto) || !Enum.IsDefined(typeof(Genre), playDto.Genre) || duration.TotalMinutes < 60)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validPlays.Add(new Play()
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = playDto.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                });

                sb.AppendLine(string.Format(SuccessfulImportPlay, playDto.Title, playDto.Genre, playDto.Rating));
            }

            context.Plays.AddRange(validPlays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            ImportCastDto[] castsDto = XmlHelper.Deserialize<ImportCastDto[]>(xmlString, "Casts");

            var sb = new StringBuilder();
            var validCasts = new List<Cast>();

            foreach (ImportCastDto castDto in castsDto)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validCasts.Add(new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                });

                sb.AppendLine(string.Format(SuccessfulImportActor, castDto.FullName, castDto.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(validCasts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            ImportTheatreDto[] theatresDto = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString)!;

            var sb = new StringBuilder();
            var validTheatres = new List<Theatre>();

            foreach (ImportTheatreDto theatreDto in theatresDto)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validTheatre = new Theatre()
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director
                };

                validTheatres.Add(validTheatre);

                foreach (ImportTicketDto ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    validTheatre.Tickets!.Add(new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId
                    });
                }

                sb.AppendLine(string.Format(SuccessfulImportTheatre, validTheatre.Name, validTheatre.Tickets.Count));
            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            int ticketsCount = theatresDto.SelectMany(t => t.Tickets).Count();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }

        private static Tuple<TimeSpan, bool> ConvertAndValidateStringToTimespan(string input)
        {
                bool isValid = false;
            try
            {
                int hours = int.Parse(input.Split(":")[0]);
                int minutes = int.Parse(input.Split(":")[1]);
                int seconds = int.Parse(input.Split(":")[2]);

                var timespan = new TimeSpan(hours, minutes, seconds);
                isValid = true;

                Tuple<TimeSpan, bool> tuple = new Tuple<TimeSpan, bool>(timespan, isValid);

                return tuple;
            }
            catch (Exception)
            {
                var timeSpan = new TimeSpan(0, 0, 0);

                Tuple<TimeSpan, bool> tuple = new Tuple<TimeSpan, bool>(timeSpan, isValid);
                return tuple;
            }
        }
    }
}
