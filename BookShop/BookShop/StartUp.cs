namespace BookShop
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net.WebSockets;
    using System.Text;

    using BookShop.Models.Enums;

    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

    using Data;

    using Initializer;

    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //Console.Write("Enter age restriction: ");
            //string cmd = Console.ReadLine()!;

            //Console.WriteLine(GetBooksByAgeRestriction(db, cmd));

            //Console.WriteLine(GetGoldenBooks(db));

            //Console.WriteLine(GetBooksByPrice(db));

            //Console.WriteLine(GetBooksNotReleasedIn(db, 1998));

            //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));

            //Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));

            //Console.WriteLine(GetAuthorNamesEndingIn(db, "sK"));

            //Console.WriteLine(GetBookTitlesContaining(db, "WOR"));

            //Console.WriteLine(GetBooksByAuthor(db, "po"));

            //Console.WriteLine($"There are {CountBooks(db, 40)} books with longer title than 40 symbols" );

            //Console.WriteLine(CountCopiesByAuthor(db));

            //Console.WriteLine(GetTotalProfitByCategory(db));

            //Console.WriteLine(GetMostRecentBooks(db));

            //IncreasePrices(db);

            //Console.WriteLine(RemoveBooks(db));
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

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorNames = context.Authors
                .Where(a => EF.Functions.Like(a.FirstName, $"%{input}"))
                .Select(a => new
                {
                    FullName = a.FirstName + ' ' + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var item in authorNames)
            {
                sb.AppendLine(item.FullName);
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var titles = context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{input}%"))
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToArray();

            return string.Join(Environment.NewLine, titles).TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var booksInfo = context.Books
                .Where(b => EF.Functions.Like(b.Author.LastName, $"{input}%"))
                .Select(b => new
                {
                    b.Title,
                    AuthorNames = $"{b.Author.FirstName} {b.Author.LastName}",
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var book in booksInfo)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorNames})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int booksCount = context.Books
                .Count(b => b.Title.Length > lengthCheck);

            return booksCount;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var totalCopiesByAuthor = context.Authors
                .Select(a => new
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    TotalCopies = a.Books.Select(b => b.Copies).Sum()
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var copy in totalCopiesByAuthor)
            {
                sb.AppendLine($"{copy.AuthorName} - {copy.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var result = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Select(cb => cb.Book.Copies * cb.Book.Price).Sum()
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var c in result)
            {
                sb.AppendLine($"{c.Name} ${c.TotalProfit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var mostRecentBooks = context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks.Select(cb => cb.Book)
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                })
                .OrderBy(c => c.Name)
                .ToArray();

            var sb = new StringBuilder();

            foreach (var c in mostRecentBooks)
            {
                sb.AppendLine($"--{c.Name}");

                foreach (var b in c.Books)
                {
                    sb.AppendLine($"{b.Title} ({b.ReleaseDate!.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate!.Value.Year < 2015)
                .ToArray();
            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();
            context.RemoveRange(books);
            context.SaveChanges();

            return books.Length;
        }
    }
}


