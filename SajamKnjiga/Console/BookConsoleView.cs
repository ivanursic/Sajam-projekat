using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class BookConsoleView
{
    private readonly BooksDAO _booksDAO;
    private readonly AuthorsDAO _authorsDAO;
    private readonly PublishersDAO _publishersDAO;
    private readonly VisitorsDAO _visitorsDAO;
    private readonly PurchasesDAO _purchasesDAO;

    public BookConsoleView(BooksDAO booksDAO, AuthorsDAO authorsDAO, PublishersDAO publishersDAO, VisitorsDAO visitorsDAO, PurchasesDAO purchasesDAO)
    {
        _booksDAO = booksDAO; _authorsDAO = authorsDAO; _publishersDAO = publishersDAO;
        _visitorsDAO = visitorsDAO; _purchasesDAO = purchasesDAO;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n=== BOOKS ===\n1: Show all books\n2: Add book\n3: Update book\n4: Remove book\n0: Back");
            switch (System.Console.ReadLine())
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
        Book b = new Book();
        System.Console.Write("ISBN: "); b.ISBN = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write("Title: "); b.Title = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write("Genre: "); b.Genre = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write("Publication year: "); b.PublicationYear = ConsoleViewUtils.SafeInputInt();
        System.Console.Write("Price: "); b.Price = ConsoleViewUtils.SafeInputDouble();
        System.Console.Write("Page count: "); b.PageCount = ConsoleViewUtils.SafeInputInt();
        do
        {
            System.Console.Write("Author IDs (comma-separated; at least one required): ");
            b.AuthorIds = ConsoleViewUtils.ReadExistingIds(_authorsDAO.GetAllAuthors().Select(a => a.Id), "author");
            if (b.AuthorIds.Count == 0) System.Console.WriteLine("A book must have at least one author.");
        } while (b.AuthorIds.Count == 0);
        System.Console.Write("Publisher ID: ");
        b.PublisherId = ConsoleViewUtils.ReadExistingId(_publishersDAO.GetAllPublishers().Select(p => p.Id), "publisher");
        b.PurchasedByVisitorIds = new List<int>();
        b.WishlistedByVisitorIds = new List<int>();
        return b;
    }

    private void AddBook()
    {
        if (_publishersDAO.GetAllPublishers().Count == 0)
        { System.Console.WriteLine("Add a publisher before creating a book."); return; }
        if (_authorsDAO.GetAllAuthors().Count == 0)
        { System.Console.WriteLine("Add at least one author before creating a book."); return; }
        Book book = InputBook();
        _booksDAO.AddBook(book);
        SynchronizeBookReferences(book);
        System.Console.WriteLine("Book added.");
    }

    private void UpdateBook()
    {
        System.Console.Write("Enter book ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_booksDAO.GetAllBooks().Select(b => b.Id), "book");
        Book b = InputBook(); b.Id = id;
        b.PurchasedByVisitorIds = _purchasesDAO.GetAllPurchases().Where(p => p.BookId == id).Select(p => p.VisitorId).Distinct().ToList();
        b.WishlistedByVisitorIds = _visitorsDAO.GetAllVisitors().Where(v => v.WishlistBookIds.Contains(id)).Select(v => v.Id).ToList();
        _booksDAO.UpdateBook(b);
        SynchronizeBookReferences(b);
        System.Console.WriteLine("Book updated.");
    }

    private void SynchronizeBookReferences(Book book)
    {
        foreach (Author author in _authorsDAO.GetAllAuthors())
        {
            if (book.AuthorIds.Contains(author.Id))
            {
                if (!author.BookIds.Contains(book.Id)) author.BookIds.Add(book.Id);
            }
            else
            {
                author.BookIds.Remove(book.Id);
            }
            _authorsDAO.UpdateAuthor(author);
        }

        foreach (Publisher publisher in _publishersDAO.GetAllPublishers())
        {
            if (publisher.Id == book.PublisherId)
            {
                if (!publisher.BookIds.Contains(book.Id)) publisher.BookIds.Add(book.Id);
            }
            else
            {
                publisher.BookIds.Remove(book.Id);
            }
            publisher.AuthorIds = _booksDAO.GetAllBooks()
                .Where(b => b.PublisherId == publisher.Id)
                .SelectMany(b => b.AuthorIds)
                .Append(publisher.HeadAuthorId)
                .Distinct()
                .ToList();
            _publishersDAO.UpdatePublisher(publisher);
        }
    }

    private void RemoveBook()
    {
        System.Console.Write("Enter book ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_booksDAO.GetAllBooks().Select(b => b.Id), "book");
        Book book = _booksDAO.GetAllBooks().First(b => b.Id == id);

        bool inUse = book.PurchasedByVisitorIds.Count > 0 ||
                     book.WishlistedByVisitorIds.Count > 0 ||
                     _purchasesDAO.GetAllPurchases().Any(p => p.BookId == id) ||
                     _visitorsDAO.GetAllVisitors().Any(v => v.WishlistBookIds.Contains(id));
        if (inUse)
        {
            System.Console.WriteLine("Book cannot be removed because a visitor has purchased or wishlisted it.");
            return;
        }

        foreach (Author author in _authorsDAO.GetAllAuthors())
        {
            author.BookIds.Remove(id);
            _authorsDAO.UpdateAuthor(author);
        }

        foreach (Publisher publisher in _publishersDAO.GetAllPublishers())
        {
            publisher.BookIds.Remove(id);
            publisher.AuthorIds = _booksDAO.GetAllBooks()
                .Where(b => b.Id != id && b.PublisherId == publisher.Id)
                .SelectMany(b => b.AuthorIds)
                .Append(publisher.HeadAuthorId)
                .Distinct()
                .ToList();
            _publishersDAO.UpdatePublisher(publisher);
        }

        _booksDAO.RemoveBook(id);
        System.Console.WriteLine("Book removed.");
    }

    private void ShowAllBooks()
    {
        List<Book> books = _booksDAO.GetAllBooks();
        if (books.Count == 0) { System.Console.WriteLine("No books found."); return; }
        foreach (Book b in books)
            System.Console.WriteLine($"ID: {b.Id}, ISBN: {b.ISBN}, Title: {b.Title}, Genre: {b.Genre}, Publication year: {b.PublicationYear}, Price: {b.Price}, Pages: {b.PageCount}, Author IDs: {string.Join(", ", b.AuthorIds)}, Publisher ID: {b.PublisherId}, Purchased by visitor IDs: {string.Join(", ", b.PurchasedByVisitorIds)}, Wishlisted by visitor IDs: {string.Join(", ", b.WishlistedByVisitorIds)}");
    }
}
