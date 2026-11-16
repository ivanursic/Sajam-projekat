using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class AuthorConsoleView
{
    private readonly AuthorsDAO _authorsDAO;

    public AuthorConsoleView(AuthorsDAO authorsDAO)
    {
        _authorsDAO = authorsDAO;
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

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1": ShowAllAuthors(); break;
                case "2": AddAuthor(); break;
                case "3": UpdateAuthor(); break;
                case "4": RemoveAuthor(); break;
                case "0": return;
                default: System.Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private Author InputAuthor()
    {
        Author author = new Author();

        System.Console.Write("First name: ");
        author.FirstName = System.Console.ReadLine() ?? "";

        System.Console.Write("Last name: ");
        author.LastName = System.Console.ReadLine() ?? "";

        System.Console.Write("Date of birth: ");
        author.DateOfBirth = ConsoleViewUtils.SafeInputDate();

        System.Console.Write("Address ID: ");
        author.AddressId = ConsoleViewUtils.SafeInputInt();

        System.Console.Write("Phone number: ");
        author.PhoneNumber = System.Console.ReadLine() ?? "";

        System.Console.Write("Email: ");
        author.Email = System.Console.ReadLine() ?? "";

        System.Console.Write("Identity card number: ");
        author.IdentityCardNumber = System.Console.ReadLine() ?? "";

        System.Console.Write("Years of experience: ");
        author.YearsOfExperience = ConsoleViewUtils.SafeInputInt();

        System.Console.Write("Book IDs (separated by commas, empty if none): ");
        author.BookIds = ConsoleViewUtils.SafeInputIds();

        return author;
    }

    private void AddAuthor()
    {
        Author author = InputAuthor();
        _authorsDAO.AddAuthor(author);
        System.Console.WriteLine("Author added.");
    }

    private void UpdateAuthor()
    {
        System.Console.Write("Enter author ID: ");
        int id = ConsoleViewUtils.SafeInputInt();

        Author author = InputAuthor();
        author.Id = id;

        _authorsDAO.UpdateAuthor(author);
        System.Console.WriteLine("Author updated if the ID exists.");
    }

    private void RemoveAuthor()
    {
        System.Console.Write("Enter author ID: ");
        int id = ConsoleViewUtils.SafeInputInt();

        _authorsDAO.RemoveAuthor(id);
        System.Console.WriteLine("Author removed if the ID exists.");
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
                $"ID: {author.Id}, Name: {author.FirstName} {author.LastName}, " +
                $"Date of birth: {author.DateOfBirth:d}, Address ID: {author.AddressId}, " +
                $"Phone: {author.PhoneNumber}, Email: {author.Email}, " +
                $"Identity card: {author.IdentityCardNumber}, " +
                $"Experience: {author.YearsOfExperience}, " +
                $"Book IDs: {string.Join(", ", author.BookIds)}");
        }
    }
}
