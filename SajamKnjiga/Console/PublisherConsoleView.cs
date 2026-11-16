using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class PublisherConsoleView
{
    private readonly PublishersDAO _publishersDao;

    public PublisherConsoleView(PublishersDAO publishersDao)
    {
        _publishersDao = publishersDao;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n--- Publishers ---");
            System.Console.WriteLine("1: Show all publishers");
            System.Console.WriteLine("2: Add publisher");
            System.Console.WriteLine("3: Update publisher");
            System.Console.WriteLine("4: Remove publisher");
            System.Console.WriteLine("0: Back");

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1":
                    ShowAllPublishers();
                    break;
                case "2":
                    AddPublisher();
                    break;
                case "3":
                    UpdatePublisher();
                    break;
                case "4":
                    RemovePublisher();
                    break;
                case "0":
                    return;
                default:
                    System.Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private void ShowAllPublishers()
    {
        foreach (Publisher publisher in _publishersDao.GetAllPublishers())
        {
            System.Console.WriteLine(
                $"ID: {publisher.Id} | Code: {publisher.Code} | " +
                $"Name: {publisher.Name} | Head author ID: {publisher.HeadAuthorId} | " +
                $"Author IDs: {string.Join(", ", publisher.AuthorIds)} | " +
                $"Book IDs: {string.Join(", ", publisher.BookIds)}"
            );
        }
    }

    private void AddPublisher()
    {
        Publisher publisher = InputPublisher();
        _publishersDao.AddPublisher(publisher);
        System.Console.WriteLine("Publisher added.");
    }

    private void UpdatePublisher()
    {
        System.Console.WriteLine("Enter publisher ID:");
        int id = ConsoleViewUtils.SafeInputInt();

        Publisher publisher = InputPublisher();
        publisher.Id = id;

        if (_publishersDao.UpdatePublisher(publisher) == null)
        {
            System.Console.WriteLine("Publisher not found.");
            return;
        }

        System.Console.WriteLine("Publisher updated.");
    }

    private void RemovePublisher()
    {
        System.Console.WriteLine("Enter publisher ID:");
        int id = ConsoleViewUtils.SafeInputInt();

        if (_publishersDao.RemovePublisher(id) == null)
        {
            System.Console.WriteLine("Publisher not found.");
            return;
        }

        System.Console.WriteLine("Publisher removed.");
    }

    private Publisher InputPublisher()
    {
        Publisher publisher = new Publisher();

        System.Console.WriteLine("Enter publisher code:");
        publisher.Code = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Enter publisher name:");
        publisher.Name = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Enter head author ID:");
        publisher.HeadAuthorId = ConsoleViewUtils.SafeInputInt();

        System.Console.WriteLine("Enter author IDs separated by commas (or leave empty):");
        publisher.AuthorIds = ReadIds();

        System.Console.WriteLine("Enter book IDs separated by commas (or leave empty):");
        publisher.BookIds = ReadIds();

        return publisher;
    }

    private List<int> ReadIds()
    {
        string input = System.Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(input))
            return new List<int>();

        return input.Split(',')
            .Select(id => int.Parse(id.Trim()))
            .ToList();
    }
}
