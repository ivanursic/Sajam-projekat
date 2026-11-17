
using SajamKnjiga.DAO;
using SajamKnjiga.Console;

namespace SajamKnjiga;

class Program
{
    static void Main()
    {
        VisitorsDAO visitors = new VisitorsDAO();
        AuthorsDAO authors = new AuthorsDAO();
        BooksDAO books = new BooksDAO();
        PurchasesDAO purchases = new PurchasesDAO();
        PublishersDAO publishers = new PublishersDAO();
        AddressesDAO addresses = new AddressesDAO();

        VisitorConsoleView visitorView =
            new VisitorConsoleView(visitors, books, purchases, addresses);

        AuthorConsoleView authorView = new AuthorConsoleView(authors, addresses, books, publishers);
        BookConsoleView bookView = new BookConsoleView(books, authors, publishers, visitors, purchases);
        PurchaseConsoleView purchaseView = new PurchaseConsoleView(purchases, visitors, books);
        PublisherConsoleView publisherView = new PublisherConsoleView(publishers, authors, books);
        AddressConsoleView addressView = new AddressConsoleView(addresses, visitors, authors);

        while (true)
        {
            System.Console.WriteLine("\n=== BOOK FAIR INFORMATION SYSTEM ===");
            System.Console.WriteLine("1: Visitors");
            System.Console.WriteLine("2: Authors");
            System.Console.WriteLine("3: Books");
            System.Console.WriteLine("4: Purchases");
            System.Console.WriteLine("5: Publishers");
            System.Console.WriteLine("6: Addresses");
            System.Console.WriteLine("0: Exit");

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1":
                    visitorView.RunMenu();
                    break;
                case "2":
                    authorView.RunMenu();
                    break;
                case "3":
                    bookView.RunMenu();
                    break;
                case "4":
                    purchaseView.RunMenu();
                    break;
                case "5":
                    publisherView.RunMenu();
                    break;
                case "6":
                    addressView.RunMenu();
                    break;
                case "0":
                    return;
                default:
                    System.Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
