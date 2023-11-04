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
            const int id = 9;
            Console.WriteLine(ExportAlbumsInfo(context, id));

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
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums)
                {
                int count = 1;
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}");
                sb.AppendLine($"-AlbumName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                if (album.Songs.Any())
                    {
                    foreach (var songs in album.Songs)
                        {
                        sb.AppendLine($"---#{count++}");
                        sb.AppendLine($"---SongName: {songs.Name}");
                        sb.AppendLine($"---Price: {songs.Price}");
                        sb.AppendLine($"---Writer: {songs.Writer.Name}");
                        }
                    }

                sb.AppendLine($"-AlbumPrice: {album.Price}");
                }

            return sb.ToString().Trim();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
