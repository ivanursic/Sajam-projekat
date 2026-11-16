using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class PurchaseConsoleView
{
    private readonly PurchasesDAO _purchasesDao;

    public PurchaseConsoleView(PurchasesDAO purchasesDao)
    {
        _purchasesDao = purchasesDao;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n--- Purchases ---");
            System.Console.WriteLine("1: Show all purchases");
            System.Console.WriteLine("2: Add purchase");
            System.Console.WriteLine("3: Update purchase");
            System.Console.WriteLine("4: Remove purchase");
            System.Console.WriteLine("0: Back");

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
            {
                case "1":
                    ShowAllPurchases();
                    break;
                case "2":
                    AddPurchase();
                    break;
                case "3":
                    UpdatePurchase();
                    break;
                case "4":
                    RemovePurchase();
                    break;
                case "0":
                    return;
                default:
                    System.Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private void ShowAllPurchases()
    {
        foreach (Purchase purchase in _purchasesDao.GetAllPurchases())
        {
            System.Console.WriteLine(
                $"ID: {purchase.Id} | Visitor ID: {purchase.VisitorId} | " +
                $"Book ID: {purchase.BookId} | Date: {purchase.PurchaseDate:d} | " +
                $"Rating: {purchase.Rating} | Comment: {purchase.Comment}"
            );
        }
    }

    private void AddPurchase()
    {
        Purchase purchase = InputPurchase();
        _purchasesDao.AddPurchase(purchase);
        System.Console.WriteLine("Purchase added.");
    }

    private void UpdatePurchase()
    {
        System.Console.WriteLine("Enter purchase ID:");
        int id = ConsoleViewUtils.SafeInputInt();

        Purchase purchase = InputPurchase();
        purchase.Id = id;

        if (_purchasesDao.UpdatePurchase(purchase) == null)
        {
            System.Console.WriteLine("Purchase not found.");
            return;
        }

        System.Console.WriteLine("Purchase updated.");
    }

    private void RemovePurchase()
    {
        System.Console.WriteLine("Enter purchase ID:");
        int id = ConsoleViewUtils.SafeInputInt();

        if (_purchasesDao.RemovePurchase(id) == null)
        {
            System.Console.WriteLine("Purchase not found.");
            return;
        }

        System.Console.WriteLine("Purchase removed.");
    }

    private Purchase InputPurchase()
    {
        Purchase purchase = new Purchase();

        System.Console.WriteLine("Enter visitor ID:");
        purchase.VisitorId = ConsoleViewUtils.SafeInputInt();

        System.Console.WriteLine("Enter book ID:");
        purchase.BookId = ConsoleViewUtils.SafeInputInt();

        System.Console.WriteLine("Enter purchase date:");
        purchase.PurchaseDate = ConsoleViewUtils.SafeInputDate();

        do
        {
            System.Console.WriteLine("Enter rating (1-5):");
            purchase.Rating = ConsoleViewUtils.SafeInputInt();
        }
        while (purchase.Rating < 1 || purchase.Rating > 5);

        System.Console.WriteLine("Enter comment:");
        purchase.Comment = System.Console.ReadLine() ?? string.Empty;

        return purchase;
    }
}