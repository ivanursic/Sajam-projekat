using SajamKnjiga.DAO;
using SajamKnjiga.Console;

namespace SajamKnjiga;

class Program
{
    static void Main()
    {
        AuthorsDAO authors = new AuthorsDAO();
        BooksDAO books = new BooksDAO();
        AddressesDAO addresses = new AddressesDAO();

        AuthorConsoleView authorView = new AuthorConsoleView(authors);
        BookConsoleView bookView = new BookConsoleView(books);
        AddressConsoleView addressView = new AddressConsoleView(addresses);

        while (true)
        {
            System.Console.WriteLine("\n=== BOOK FAIR INFORMATION SYSTEM ===");
            System.Console.WriteLine("1: Authors");
            System.Console.WriteLine("2: Books");
            System.Console.WriteLine("3: Addresses");
            System.Console.WriteLine("0: Exit");

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1": authorView.RunMenu(); break;
                case "2": bookView.RunMenu(); break;
                case "3": addressView.RunMenu(); break;
                case "0": return;
                default: System.Console.WriteLine("Invalid option."); break;
            }
        }
    }
}
