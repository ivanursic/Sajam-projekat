using SajamKnjiga.DAO;
using SajamKnjiga.Console;

namespace SajamKnjiga;

class Program
{
    static void Main()
    {
        VisitorsDAO visitors = new VisitorsDAO();
        PurchasesDAO purchases = new PurchasesDAO();
        PublishersDAO publishers = new PublishersDAO();

        VisitorConsoleView visitorView = new VisitorConsoleView(visitors);
        PurchaseConsoleView purchaseView = new PurchaseConsoleView(purchases);
        PublisherConsoleView publisherView = new PublisherConsoleView(publishers);

        while (true)
        {
            System.Console.WriteLine("\n=== BOOK FAIR INFORMATION SYSTEM ===");
            System.Console.WriteLine("1: Visitors");
            System.Console.WriteLine("2: Purchases");
            System.Console.WriteLine("3: Publishers");
            System.Console.WriteLine("0: Exit");

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1":
                    visitorView.RunMenu();
                    break;
                case "2":
                    purchaseView.RunMenu();
                    break;
                case "3":
                    publisherView.RunMenu();
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