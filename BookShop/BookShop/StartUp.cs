namespace BookShop
{
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

            Console.WriteLine(GetGoldenBooks(db));
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
    }
}


