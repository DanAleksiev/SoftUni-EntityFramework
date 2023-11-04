namespace MusicHub
    {
    using System;
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

            //var producer = context.Producers.First();
            //Console.WriteLine(producer.Name);
            //DbInitializer.ResetDatabase(context);

            //Test your solutions here
            const int id = 4;
            Console.WriteLine(ExportSongsAboveDuration(context, id));

            }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
            {
            var albums = context.Albums
                .Include(x => x.Songs)
                .ThenInclude(x => x.Writer)
                .Select(a => new
                    {
                    a.Name,
                    a.ProducerId,
                    a.ReleaseDate,
                    a.Price,
                    a.Songs,
                    ProducerName = a.Producer.Name,
                    })
                .Where(a => a.ProducerId == producerId)
                .ToList()
                .OrderByDescending(a => a.Price);

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums)
                {
                int count = 1;
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                if (album.Songs.Any())
                    {
                    foreach (var songs in album.Songs
                        .OrderByDescending(s => s.Name)
                        .ThenBy(w => w.Writer.Name))
                        {
                        sb.AppendLine($"---#{count++}");
                        sb.AppendLine($"---SongName: {songs.Name}");
                        sb.AppendLine($"---Price: {songs.Price:f2}");
                        sb.AppendLine($"---Writer: {songs.Writer.Name}");
                        }
                    }

                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
                }

            return sb.ToString().Trim();
            }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
            {
            var songs = context.Songs
                .Include(s => s.SongPerformers)
                .ThenInclude(s => s.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                    .ThenInclude(a => a.Producer)
                .ToList()
                .Where(s => s.Duration.TotalSeconds > 4)
                .Select(s => new
                    {
                    s.Name,
                    Performer = s.SongPerformers
                    .Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}")
                    .ToList(),
                    WriterName = s.Writer.Name,
                    Producer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c"),
                    })
                .OrderBy(s => s.Name)
                .ThenBy(s => s.WriterName);


            StringBuilder sb = new StringBuilder();

            int counter = 1;
            foreach (var s in songs)
                {
                sb.AppendLine($"-Song #{counter++}");
                sb.AppendLine($"--SongName: {s.Name}");
                sb.AppendLine($"--Writer: {s.WriterName}");
                if (s.Performer.Any())
                    {
                    foreach (var p in s.Performer)
                        {

                        sb.AppendLine($"--Performer: {p}");
                        }
                    }

                sb.AppendLine($"--AlbumProducer: {s.Producer}");
                sb.AppendLine($"--Duration: {s.Duration}");


                }




            return sb.ToString().Trim();
            }
        }
    }
