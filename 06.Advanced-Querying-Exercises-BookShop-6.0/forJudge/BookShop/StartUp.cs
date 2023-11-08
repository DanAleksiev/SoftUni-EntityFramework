namespace BookShop
    {
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
    using static System.Reflection.Metadata.BlobBuilder;

    public class StartUp
        {
        public static void Main()
            {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //2
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, input));

            //3
            //Console.WriteLine(GetGoldenBooks(db));

            //4
            //Console.WriteLine(GetBooksByPrice(db));

            //5
            //int input = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db,input));


            //6
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, input));

            //7
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, input));

            //8
            //string input = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

            //9
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, input));

            //10
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, input));

            //11
            //int input = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(db, input));

            //12
            //Console.WriteLine(CountCopiesByAuthor(db));

            //13
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //14
            //Console.WriteLine(GetMostRecentBooks(db));

            //15
            //IncreasePrices(db);

            //16
            Console.WriteLine(RemoveBooks(db));
        }

        //2
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
            {

            if (!Enum.TryParse<AgeRestriction>(command, true, out var ageRestriction))
                {
                return $"{command} is not a valid age restriction";
                }


            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => new
                    {
                    b.Title,
                    })
                .OrderBy(b => b.Title)
                .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
                {
                sb.AppendLine(book.Title);
                }

            return sb.ToString().Trim();
            }

        //3
        public static string GetGoldenBooks(BookShopContext context)
            {
            var books = context.Books
                .Where(b => b.Copies < 5000 && b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .ToList();


            return string.Join(Environment.NewLine, books.Select(b => b.Title));
            }


        //4
        public static string GetBooksByPrice(BookShopContext context)
            {
            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - ${b.Price:f2}"));
            }

        //5
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
            {
            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title}"));
            }

        //6
        public static string GetBooksByCategory(BookShopContext context, string input)
            {
            string[] categoryes = input
                .Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Select(b => b.ToLower())
                .ToArray();

            var books = context.Books
                .Select(b => new
                    {
                    b.Title,
                    b.BookCategories,
                    })
                .Where(b => b.BookCategories.Any(bc => categoryes.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title}"));
            }


        //7
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
            {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Select(b => new
                    {
                    b.Title,
                    b.ReleaseDate,
                    b.EditionType,
                    b.Price
                })
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}"));
            }

        //8
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
            {
            var authors = context.Authors
                .Select(a => new
                    {
                    a.FirstName,
                    a.LastName,
                    })
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .ToList();

            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FirstName} {a.LastName}"));
            }

        //9
        public static string GetBookTitlesContaining(BookShopContext context, string input)
            {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title}"));
            }

        //10
        public static string GetBooksByAuthor(BookShopContext context, string input)
            {
            var books = context.Books
                .Include(b => b.Author)
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})"));
            }

        //11
        public static int CountBooks(BookShopContext context, int lengthCheck)
            {
            var books =  context.Books
                 .Where(b => b.Title.Length > lengthCheck)
                 .Count();

            return books;
            }

        //12
        public static string CountCopiesByAuthor(BookShopContext context)
            {
            var author = context.Authors
                .Include(a => a.Books)
                .Select(a => new
                    {
                    FullName = $"{a.FirstName} {a.LastName}",
                    Coppies = a.Books.Sum(b => b.Copies)
                    })
            .OrderByDescending(b => b.Coppies)
            .ToList();

            return string.Join(Environment.NewLine, author.Select(b => $"{b.FullName} - {b.Coppies}"));
            }

        //13
        public static string GetTotalProfitByCategory(BookShopContext context)
            {
            var categorys = context.Categories
                .AsNoTracking()
                .Select(c => new
                    {
                    c.Name,
                    Sum = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                    })
                .OrderByDescending(c => c.Sum)
                .ThenBy(c => c.Name);

            return string.Join(Environment.NewLine, categorys.Select(b => $"{b.Name} ${b.Sum:f2}"));
            }

        //14
        public static string GetMostRecentBooks(BookShopContext context)
            {

            var categories = context.Categories
                .AsNoTracking()
                .Select(c => new
                    {
                    CatName = c.Name,
                    Books = c.CategoryBooks.Select(cb => new
                        {
                        RDate = cb.Book.ReleaseDate,
                        BTitle = cb.Book.Title
                        })
                    .OrderByDescending(c => c.RDate)
                    .Take(3)
                    .ToArray()
                    })
                .OrderBy(c => c.CatName)
                .ToArray();


            StringBuilder sb = new StringBuilder();

            foreach (var c in categories)
                {
                sb.AppendLine($"--{c.CatName}");
                foreach (var b in c.Books)
                    {
                    sb.AppendLine($"{b.BTitle} ({b.RDate.Value.Year})");
                    }
                }
            return sb.ToString().Trim();
            }

        //15
        public static void IncreasePrices(BookShopContext context)
            {
            const int year = 2010;
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < year);


            foreach (var book in books)
                {
                book.Price += 5;
                }

            context.SaveChanges();
            }


        //16
        public static int RemoveBooks(BookShopContext context)
            {
            var books = context.Books
                .Where(b => b.Copies <= 4200);

            int count = books.Count();

            context.RemoveRange(books);
            context.SaveChanges();

            return count;
            }
        }
    }


