namespace BookShop
{
    using System.Globalization;
    using System.Text;

    using BookShop.Models.Enums;

    using Data;

    using Initializer;

    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.Write("Enter age restriction: ");
            //string cmd = Console.ReadLine()!;

            //Console.WriteLine(GetBooksByAgeRestriction(db, cmd));

            //Console.WriteLine(GetGoldenBooks(db));

            //Console.WriteLine(GetBooksByPrice(db));

            //Console.WriteLine(GetBooksNotReleasedIn(db, 1998));

            //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));

            Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));

        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var bookTitles = context.Books
                .AsNoTracking()
                .ToArray()
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles).Trim();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] goldenBooksTitles = context.Books
                .AsNoTracking()
                .Where(b => b.EditionType.Equals(EditionType.Gold) && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, goldenBooksTitles);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var bookInfo = context.Books
                .AsNoTracking()
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var book in bookInfo)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksInfo = context.Books
                .AsNoTracking()
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, booksInfo).Trim();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categoriesList = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var titles = context.Books
                .Select(b => new
                {
                    b.Title,
                    Cat = b.BookCategories.Where(bc => categoriesList.Contains(bc.Category.Name.ToLower())).ToArray()
                })
                .Where(c => c.Cat.Length > 0)
                .Select(b => b.Title)
                //.Distinct()
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, titles).TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime stringToDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var booksInfo = context.Books
                .Where(b => b.ReleaseDate < stringToDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var book in booksInfo)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}


