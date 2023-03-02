namespace MusicHub
{
    using System;
    using System.Data.Entity;
    using System.Text;

    using Data;

    using Initializer;

    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            using (context)
            {
                var result = context.Producers
                    .ToArray()
                    .Where(p => p.Id == producerId)
                    .Select(p => p.Albums
                                .Select(a => new
                                {
                                    ProducerName = a.Producer!.Name,
                                    AlbumName = a.Name,
                                    a.ReleaseDate,
                                    Songs = a.Songs
                                        .Select(s => new
                                        {
                                            SongName = s.Name,
                                            s.Price,
                                            SongWriter = s.Writer!.Name
                                        })
                                        .OrderByDescending(s => s.SongName)
                                        .ThenBy(s => s.SongWriter)
                                        .ToArray(),
                                    TotalAlbumPrice = a.Price
                                })
                                .OrderByDescending(a => a.TotalAlbumPrice)
                                .ToArray()
                    ).ToArray();

                var sb = new StringBuilder();

                foreach (var albumList in result!)
                {
                    foreach (var album in albumList)
                    {
                        sb.AppendLine($"-AlbumName: {album.AlbumName}");
                        sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                        sb.AppendLine($"-ProducerName: {album.ProducerName}");
                        sb.AppendLine($"-Songs:");

                        var songsArray = album.Songs.ToArray();

                        for (int i = 0; i < songsArray.Length; i++)
                        {
                            sb.AppendLine($"---#{i + 1}");
                            sb.AppendLine($"---SongName: {songsArray[i].SongName}");
                            sb.AppendLine($"---Price: {songsArray[i].Price:f2}");
                            sb.AppendLine($"---Writer: {songsArray[i].SongWriter}");
                        }
                        sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");
                    }
                }

                return sb.ToString().TrimEnd();
            }
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songsExport = context.Songs
                .ToList()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                        .Select(sp => new
                        {
                            FullName = $"{sp.Performer.FirstName} {sp.Performer.LastName}"
                        })

                        .OrderBy(p => p.FullName),
                    WriterName = s.Writer!.Name,
                    AlbumProducer = s.Album!.Producer!.Name,
                    s.Duration
                })
                .OrderBy(s => s.Name)
                .ThenBy(w => w.Name)
                .ToArray();

            var sb = new StringBuilder();

            int counter = 1;

            foreach (var song in songsExport)
            {
                sb.AppendLine($"-Song #{counter}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.WriterName}");

                foreach (var performer in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performer.FullName}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration.ToString("c")}");

                counter++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
