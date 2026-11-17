
using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class AuthorConsoleView
{
    private readonly AuthorsDAO _authorsDAO;
    private readonly AddressesDAO _addressesDAO;
    private readonly BooksDAO _booksDAO;
    private readonly PublishersDAO _publishersDAO;

    public AuthorConsoleView(
        AuthorsDAO authorsDAO,
        AddressesDAO addressesDAO,
        BooksDAO booksDAO,
        PublishersDAO publishersDAO)
    {
        _authorsDAO = authorsDAO;
        _addressesDAO = addressesDAO;
        _booksDAO = booksDAO;
        _publishersDAO = publishersDAO;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n=== AUTHORS ===");
            System.Console.WriteLine("1: Show all authors");
            System.Console.WriteLine("2: Add author");
            System.Console.WriteLine("3: Update author");
            System.Console.WriteLine("4: Remove author");
            System.Console.WriteLine("0: Back");

            switch (System.Console.ReadLine())
            {
                case "1":
                    ShowAllAuthors();
                    break;
                case "2":
                    AddAuthor();
                    break;
                case "3":
                    UpdateAuthor();
                    break;
                case "4":
                    RemoveAuthor();
                    break;
                case "0":
                    return;
                default:
                    System.Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private Author InputAuthor()
    {
        Author author = new Author();

        System.Console.WriteLine("First name:");
        author.FirstName = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Last name:");
        author.LastName = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Date of birth:");
        author.DateOfBirth = ConsoleViewUtils.SafeInputDate();

        System.Console.WriteLine("Address ID:");
        author.AddressId = ConsoleViewUtils.ReadExistingId(
            _addressesDAO.GetAllAddresses().Select(a => a.Id),
            "address"
        );

        System.Console.WriteLine("Phone number:");
        author.PhoneNumber = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Email:");
        author.Email = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Identity card number:");
        author.IdentityCardNumber = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Years of experience:");
        author.YearsOfExperience = ConsoleViewUtils.SafeInputInt();

        System.Console.WriteLine("Book IDs (comma-separated, empty if none):");
        author.BookIds = ConsoleViewUtils.ReadExistingIds(
            _booksDAO.GetAllBooks().Select(b => b.Id),
            "book"
        );

        return author;
    }

    private void AddAuthor()
    {
        if (_addressesDAO.GetAllAddresses().Count == 0)
        {
            System.Console.WriteLine(
                "Add an address before creating an author."
            );
            return;
        }

        Author author = InputAuthor();

        _authorsDAO.AddAuthor(author);
        SynchronizeAuthorBooks(author);

        System.Console.WriteLine("Author added.");
    }

    private void UpdateAuthor()
    {
        System.Console.WriteLine("Enter author ID:");

        int id = ConsoleViewUtils.ReadExistingId(
            _authorsDAO.GetAllAuthors().Select(a => a.Id),
            "author"
        );

        Author author = InputAuthor();
        author.Id = id;

        if (author.YearsOfExperience < 5 &&
            _publishersDAO.GetAllPublishers().Any(p => p.HeadAuthorId == id))
        {
            System.Console.WriteLine(
                "This author is a publisher's head author and must retain at least five years of experience. Update cancelled."
            );
            return;
        }

        _authorsDAO.UpdateAuthor(author);
        SynchronizeAuthorBooks(author);

        System.Console.WriteLine("Author updated.");
    }

    private void SynchronizeAuthorBooks(Author author)
    {
        foreach (Book book in _booksDAO.GetAllBooks())
        {
            if (author.BookIds.Contains(book.Id))
            {
                if (!book.AuthorIds.Contains(author.Id))
                    book.AuthorIds.Add(author.Id);
            }
            else
            {
                book.AuthorIds.Remove(author.Id);
            }

            _booksDAO.UpdateBook(book);
        }

        foreach (Publisher publisher in _publishersDAO.GetAllPublishers())
        {
            publisher.AuthorIds = _booksDAO.GetAllBooks()
                .Where(b => b.PublisherId == publisher.Id)
                .SelectMany(b => b.AuthorIds)
                .Append(publisher.HeadAuthorId)
                .Distinct()
                .ToList();

            publisher.BookIds = _booksDAO.GetAllBooks()
                .Where(b => b.PublisherId == publisher.Id)
                .Select(b => b.Id)
                .ToList();

            _publishersDAO.UpdatePublisher(publisher);
        }
    }

    private void RemoveAuthor()
    {
        System.Console.WriteLine("Enter author ID:");

        int id = ConsoleViewUtils.ReadExistingId(
            _authorsDAO.GetAllAuthors().Select(a => a.Id),
            "author"
        );

        bool inUse =
            _authorsDAO.GetAllAuthors().First(a => a.Id == id).BookIds.Count > 0 ||
            _booksDAO.GetAllBooks().Any(b => b.AuthorIds.Contains(id)) ||
            _publishersDAO.GetAllPublishers().Any(
                p => p.HeadAuthorId == id || p.AuthorIds.Contains(id)
            );

        if (inUse)
        {
            System.Console.WriteLine(
                "Author cannot be removed because a book or publisher references this author."
            );
            return;
        }

        _authorsDAO.RemoveAuthor(id);
        System.Console.WriteLine("Author removed.");
    }

    private void ShowAllAuthors()
    {
        List<Author> authors = _authorsDAO.GetAllAuthors();

        if (authors.Count == 0)
        {
            System.Console.WriteLine("No authors found.");
            return;
        }

        foreach (Author author in authors)
        {
            System.Console.WriteLine(
                $"ID: {author.Id} | " +
                $"Name: {author.FirstName} {author.LastName} | " +
                $"Date of birth: {author.DateOfBirth:d} | " +
                $"Address ID: {author.AddressId} | " +
                $"Phone: {author.PhoneNumber} | " +
                $"Email: {author.Email} | " +
                $"Identity card: {author.IdentityCardNumber} | " +
                $"Experience: {author.YearsOfExperience} | " +
                $"Book IDs: {string.Join(", ", author.BookIds)}"
            );
        }
    }
}
