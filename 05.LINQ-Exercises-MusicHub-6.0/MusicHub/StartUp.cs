namespace MusicHub
    {
    using System;

    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.Models;

    public class StartUp
        {
        public static void Main()
            {
            MusicHubDbContext context =
                new MusicHubDbContext();

            //var producer = context.Producers.First();
            Console.WriteLine(context.Producers.FirstOrDefault().Id);

            // DbInitializer.ResetDatabase(context);
            const int id = 9;
            ExportAlbumsInfo(context, id);

            }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
            {
            var al = context.Albums
                .Select(a => new
                    {
                    a.Name,
                    a.ReleaseDate,
                    a.Price,
                    a.ProducerId
                    })
                .Where(p => p.ProducerId == producerId).ToArray();

            Console.WriteLine(al);


            return "";
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
            {
            throw new NotImplementedException();
            }
        }
    }
