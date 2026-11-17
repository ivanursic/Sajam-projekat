using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class PublisherConsoleView
{
    private readonly PublishersDAO _publishersDao;
    private readonly AuthorsDAO _authorsDao;
    private readonly BooksDAO _booksDao;

    public PublisherConsoleView(PublishersDAO publishersDao, AuthorsDAO authorsDao, BooksDAO booksDao)
    {
        _publishersDao = publishersDao; _authorsDao = authorsDao; _booksDao = booksDao;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n--- Publishers ---\n1: Show all publishers\n2: Add publisher\n3: Update publisher\n4: Remove publisher\n0: Back");
            switch (System.Console.ReadLine())
            {
                case "1": ShowAllPublishers(); break;
                case "2": AddPublisher(); break;
                case "3": UpdatePublisher(); break;
                case "4": RemovePublisher(); break;
                case "0": return;
                default: System.Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private Publisher InputPublisher(int? existingPublisherId = null)
    {
        Publisher p = new Publisher();
        System.Console.Write("Enter publisher code: "); p.Code = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write("Enter publisher name: "); p.Name = System.Console.ReadLine() ?? string.Empty;
        System.Console.Write("Enter head author ID (author must have at least 5 years of experience): ");
        p.HeadAuthorId = ConsoleViewUtils.ReadExistingId(_authorsDao.GetAllAuthors().Where(a => a.YearsOfExperience >= 5).Select(a => a.Id), "eligible head author (5+ years experience)");
        p.AuthorIds = existingPublisherId.HasValue
            ? _booksDao.GetAllBooks().Where(b => b.PublisherId == existingPublisherId.Value)
                .SelectMany(b => b.AuthorIds).Append(p.HeadAuthorId).Distinct().ToList()
            : new List<int> { p.HeadAuthorId };
        p.BookIds = existingPublisherId.HasValue
            ? _booksDao.GetAllBooks().Where(b => b.PublisherId == existingPublisherId.Value)
                .Select(b => b.Id).ToList()
            : new List<int>();
        return p;
    }

    private void AddPublisher()
    {
        if (!_authorsDao.GetAllAuthors().Any(a => a.YearsOfExperience >= 5))
        { System.Console.WriteLine("Add an author with at least five years of experience before creating a publisher."); return; }
        _publishersDao.AddPublisher(InputPublisher());
        System.Console.WriteLine("Publisher added.");
    }

    private void UpdatePublisher()
    {
        System.Console.Write("Enter publisher ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_publishersDao.GetAllPublishers().Select(p => p.Id), "publisher");
        Publisher p = InputPublisher(id); p.Id = id;
        _publishersDao.UpdatePublisher(p); System.Console.WriteLine("Publisher updated.");
    }

    private void RemovePublisher()
    {
        System.Console.Write("Enter publisher ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_publishersDao.GetAllPublishers().Select(p => p.Id), "publisher");
        if (_booksDao.GetAllBooks().Any(b => b.PublisherId == id))
        { System.Console.WriteLine("Publisher cannot be removed because a book references it."); return; }
        _publishersDao.RemovePublisher(id); System.Console.WriteLine("Publisher removed.");
    }

    private void ShowAllPublishers()
    {
        List<Publisher> list = _publishersDao.GetAllPublishers();
        if (list.Count == 0) { System.Console.WriteLine("No publishers found."); return; }
        foreach (Publisher p in list)
            System.Console.WriteLine($"ID: {p.Id} | Code: {p.Code} | Name: {p.Name} | Head author ID: {p.HeadAuthorId} | Author IDs: {string.Join(", ", p.AuthorIds)} | Book IDs: {string.Join(", ", p.BookIds)}");
    }
}
