using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class BookConsoleView
{
    private readonly BooksDAO _booksDAO;

    public BookConsoleView(BooksDAO booksDAO)
    {
        _booksDAO = booksDAO;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n=== BOOKS ===");
            System.Console.WriteLine("1: Show all books");
            System.Console.WriteLine("2: Add book");
            System.Console.WriteLine("3: Update book");
            System.Console.WriteLine("4: Remove book");
            System.Console.WriteLine("0: Back");

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1": ShowAllBooks(); break;
                case "2": AddBook(); break;
                case "3": UpdateBook(); break;
                case "4": RemoveBook(); break;
                case "0": return;
                default: System.Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private Book InputBook()
    {
        Book book = new Book();

        System.Console.Write("ISBN: ");
        book.ISBN = System.Console.ReadLine() ?? "";

        System.Console.Write("Title: ");
        book.Title = System.Console.ReadLine() ?? "";

        System.Console.Write("Genre: ");
        book.Genre = System.Console.ReadLine() ?? "";

        System.Console.Write("Publication year: ");
        book.PublicationYear = ConsoleViewUtils.SafeInputInt();

        System.Console.Write("Price: ");
        book.Price = ConsoleViewUtils.SafeInputDouble();

        System.Console.Write("Page count: ");
        book.PageCount = ConsoleViewUtils.SafeInputInt();

        System.Console.Write("Author IDs (separated by commas): ");
        book.AuthorIds = ConsoleViewUtils.SafeInputIds();

        System.Console.Write("Publisher ID: ");
        book.PublisherId = ConsoleViewUtils.SafeInputInt();

        System.Console.Write("Visitor IDs who purchased the book (separated by commas): ");
        book.PurchasedByVisitorIds = ConsoleViewUtils.SafeInputIds();

        System.Console.Write("Visitor IDs who wishlisted the book (separated by commas): ");
        book.WishlistedByVisitorIds = ConsoleViewUtils.SafeInputIds();

        return book;
    }

    private void AddBook()
    {
        Book book = InputBook();
        _booksDAO.AddBook(book);
        System.Console.WriteLine("Book added.");
    }

    private void UpdateBook()
    {
        System.Console.Write("Enter book ID: ");
        int id = ConsoleViewUtils.SafeInputInt();

        Book book = InputBook();
        book.Id = id;

        _booksDAO.UpdateBook(book);
        System.Console.WriteLine("Book updated if the ID exists.");
    }

    private void RemoveBook()
    {
        System.Console.Write("Enter book ID: ");
        int id = ConsoleViewUtils.SafeInputInt();

        _booksDAO.RemoveBook(id);
        System.Console.WriteLine("Book removed if the ID exists.");
    }

    private void ShowAllBooks()
    {
        List<Book> books = _booksDAO.GetAllBooks();

        if (books.Count == 0)
        {
            System.Console.WriteLine("No books found.");
            return;
        }

        foreach (Book book in books)
        {
            System.Console.WriteLine(
                $"ID: {book.Id}, ISBN: {book.ISBN}, Title: {book.Title}, " +
                $"Genre: {book.Genre}, Publication year: {book.PublicationYear}, " +
                $"Price: {book.Price}, Pages: {book.PageCount}, " +
                $"Author IDs: {string.Join(", ", book.AuthorIds)}, " +
                $"Publisher ID: {book.PublisherId}, " +
                $"Purchased by visitor IDs: {string.Join(", ", book.PurchasedByVisitorIds)}, " +
                $"Wishlisted by visitor IDs: {string.Join(", ", book.WishlistedByVisitorIds)}");
        }
    }
}
