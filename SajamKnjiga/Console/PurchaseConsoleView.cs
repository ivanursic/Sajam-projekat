using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class PurchaseConsoleView
{
    private readonly PurchasesDAO _purchasesDao;
    private readonly VisitorsDAO _visitorsDao;
    private readonly BooksDAO _booksDao;

    public PurchaseConsoleView(PurchasesDAO purchasesDao, VisitorsDAO visitorsDao, BooksDAO booksDao)
    { _purchasesDao = purchasesDao; _visitorsDao = visitorsDao; _booksDao = booksDao; }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n--- Purchases ---\n1: Show all purchases\n2: Add purchase\n3: Update purchase\n4: Remove purchase\n0: Back");
            switch (System.Console.ReadLine())
            {
                case "1": ShowAllPurchases(); break;
                case "2": AddPurchase(); break;
                case "3": UpdatePurchase(); break;
                case "4": RemovePurchase(); break;
                case "0": return;
                default: System.Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private Purchase InputPurchase()
    {
        Purchase p = new Purchase();
        System.Console.Write("Enter visitor ID: ");
        p.VisitorId = ConsoleViewUtils.ReadExistingId(_visitorsDao.GetAllVisitors().Select(v => v.Id), "visitor");
        System.Console.Write("Enter book ID: ");
        p.BookId = ConsoleViewUtils.ReadExistingId(_booksDao.GetAllBooks().Select(b => b.Id), "book");
        System.Console.Write("Enter purchase date: "); p.PurchaseDate = ConsoleViewUtils.SafeInputDate();
        do
        {
            System.Console.Write("Enter rating (1-5): "); p.Rating = ConsoleViewUtils.SafeInputInt();
            if (p.Rating < 1 || p.Rating > 5) System.Console.WriteLine("Rating must be between 1 and 5.");
        } while (p.Rating < 1 || p.Rating > 5);
        System.Console.Write("Enter comment: "); p.Comment = System.Console.ReadLine() ?? string.Empty;
        return p;
    }

    private void AddPurchase()
    {
        if (_visitorsDao.GetAllVisitors().Count == 0 || _booksDao.GetAllBooks().Count == 0)
        { System.Console.WriteLine("Add at least one visitor and one book before creating a purchase."); return; }
        Purchase p = _purchasesDao.AddPurchase(InputPurchase());
        Visitor visitor = _visitorsDao.GetAllVisitors().First(v => v.Id == p.VisitorId);
        if (!visitor.PurchaseIds.Contains(p.Id)) visitor.PurchaseIds.Add(p.Id);
        _visitorsDao.UpdateVisitor(visitor);
        Book book = _booksDao.GetAllBooks().First(b => b.Id == p.BookId);
        if (!book.PurchasedByVisitorIds.Contains(p.VisitorId)) book.PurchasedByVisitorIds.Add(p.VisitorId);
        _booksDao.UpdateBook(book);
        System.Console.WriteLine("Purchase added.");
    }

    private void UpdatePurchase()
    {
        System.Console.Write("Enter purchase ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_purchasesDao.GetAllPurchases().Select(p => p.Id), "purchase");
        Purchase old = _purchasesDao.GetAllPurchases().First(p => p.Id == id);

        System.Console.Write("Enter new purchase date: ");
        old.PurchaseDate = ConsoleViewUtils.SafeInputDate();
        do
        {
            System.Console.Write("Enter new rating (1-5): ");
            old.Rating = ConsoleViewUtils.SafeInputInt();
            if (old.Rating < 1 || old.Rating > 5) System.Console.WriteLine("Rating must be between 1 and 5.");
        } while (old.Rating < 1 || old.Rating > 5);
        System.Console.Write("Enter new comment: ");
        old.Comment = System.Console.ReadLine() ?? string.Empty;

        _purchasesDao.UpdatePurchase(old);
        System.Console.WriteLine("Purchase updated.");
    }

    private void RemovePurchase()
    {
        System.Console.Write("Enter purchase ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_purchasesDao.GetAllPurchases().Select(p => p.Id), "purchase");
        Purchase purchase = _purchasesDao.GetAllPurchases().First(p => p.Id == id);
        _purchasesDao.RemovePurchase(id);
        Visitor visitor = _visitorsDao.GetAllVisitors().First(v => v.Id == purchase.VisitorId);
        visitor.PurchaseIds.Remove(id); _visitorsDao.UpdateVisitor(visitor);
        bool visitorStillBoughtBook = _purchasesDao.GetAllPurchases().Any(p => p.VisitorId == purchase.VisitorId && p.BookId == purchase.BookId);
        if (!visitorStillBoughtBook)
        {
            Book book = _booksDao.GetAllBooks().First(b => b.Id == purchase.BookId);
            book.PurchasedByVisitorIds.Remove(purchase.VisitorId); _booksDao.UpdateBook(book);
        }
        System.Console.WriteLine("Purchase removed.");
    }

    private void ShowAllPurchases()
    {
        List<Purchase> list = _purchasesDao.GetAllPurchases();
        if (list.Count == 0) { System.Console.WriteLine("No purchases found."); return; }
        foreach (Purchase p in list)
            System.Console.WriteLine($"ID: {p.Id} | Visitor ID: {p.VisitorId} | Book ID: {p.BookId} | Date: {p.PurchaseDate:d} | Rating: {p.Rating} | Comment: {p.Comment}");
    }
}
