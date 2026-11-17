
using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class VisitorConsoleView
{
    private readonly VisitorsDAO _visitorsDao;
    private readonly BooksDAO _booksDao;
    private readonly PurchasesDAO _purchasesDao;
    private readonly AddressesDAO _addressesDao;

    public VisitorConsoleView(
        VisitorsDAO visitorsDao,
        BooksDAO booksDao,
        PurchasesDAO purchasesDao,
        AddressesDAO addressesDao)
    {
        _visitorsDao = visitorsDao;
        _booksDao = booksDao;
        _purchasesDao = purchasesDao;
        _addressesDao = addressesDao;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine(
                "\n--- Visitors ---\n" +
                "1: Show all visitors\n" +
                "2: Add visitor\n" +
                "3: Update visitor\n" +
                "4: Remove visitor\n" +
                "0: Back"
            );

            switch (System.Console.ReadLine())
            {
                case "1":
                    ShowAllVisitors();
                    break;
                case "2":
                    AddVisitor();
                    break;
                case "3":
                    UpdateVisitor();
                    break;
                case "4":
                    RemoveVisitor();
                    break;
                case "0":
                    return;
                default:
                    System.Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private Visitor InputVisitor(int? existingVisitorId = null)
    {
        Visitor v = new Visitor();

        System.Console.Write("First name: ");
        v.FirstName = System.Console.ReadLine() ?? string.Empty;

        System.Console.Write("Last name: ");
        v.LastName = System.Console.ReadLine() ?? string.Empty;

        System.Console.Write("Date of birth: ");
        v.DateOfBirth = ConsoleViewUtils.SafeInputDate();

        System.Console.Write("Address ID: ");
        v.AddressId = ConsoleViewUtils.ReadExistingId(
            _addressesDao.GetAllAddresses().Select(a => a.Id),
            "address"
        );

        System.Console.Write("Phone number: ");
        v.PhoneNumber = System.Console.ReadLine() ?? string.Empty;

        System.Console.Write("Email: ");
        v.Email = System.Console.ReadLine() ?? string.Empty;

        System.Console.Write("Membership card number: ");
        v.MembershipCardNumber = System.Console.ReadLine() ?? string.Empty;

        System.Console.Write("Membership year: ");
        v.MembershipYear = ConsoleViewUtils.SafeInputInt();

        System.Console.Write("Status (Regular/VIP): ");
        v.Status = ConsoleViewUtils.ReadVisitorStatus();

        System.Console.Write("Average review rating (0-5): ");
        v.AverageReviewRating = ConsoleViewUtils.SafeInputAverageRating();

        v.PurchaseIds = existingVisitorId.HasValue
            ? _purchasesDao.GetAllPurchases()
                .Where(p => p.VisitorId == existingVisitorId.Value)
                .Select(p => p.Id)
                .ToList()
            : new List<int>();

        System.Console.Write("Wishlist book IDs (comma-separated, empty if none): ");
        v.WishlistBookIds = ConsoleViewUtils.ReadExistingIds(
            _booksDao.GetAllBooks().Select(b => b.Id),
            "book"
        );

        return v;
    }

    private void AddVisitor()
    {
        if (_addressesDao.GetAllAddresses().Count == 0)
        {
            System.Console.WriteLine(
                "Add an address before creating a visitor."
            );
            return;
        }

        Visitor visitor = _visitorsDao.AddVisitor(InputVisitor());

        SynchronizeWishlist(visitor.Id, visitor.WishlistBookIds);

        System.Console.WriteLine("Visitor added.");
    }

    private void UpdateVisitor()
    {
        System.Console.Write("Enter visitor ID: ");

        int id = ConsoleViewUtils.ReadExistingId(
            _visitorsDao.GetAllVisitors().Select(v => v.Id),
            "visitor"
        );

        Visitor v = InputVisitor(id);
        v.Id = id;

        _visitorsDao.UpdateVisitor(v);

        SynchronizeWishlist(id, v.WishlistBookIds);

        System.Console.WriteLine("Visitor updated.");
    }

    private void SynchronizeWishlist(
        int visitorId,
        List<int> wishlistBookIds)
    {
        foreach (Book book in _booksDao.GetAllBooks())
        {
            if (wishlistBookIds.Contains(book.Id))
            {
                if (!book.WishlistedByVisitorIds.Contains(visitorId))
                    book.WishlistedByVisitorIds.Add(visitorId);
            }
            else
            {
                book.WishlistedByVisitorIds.Remove(visitorId);
            }

            _booksDao.UpdateBook(book);
        }
    }

    private void RemoveVisitor()
    {
        System.Console.Write("Enter visitor ID: ");

        int id = ConsoleViewUtils.ReadExistingId(
            _visitorsDao.GetAllVisitors().Select(v => v.Id),
            "visitor"
        );

        bool inUse =
            _visitorsDao.GetAllVisitors()
                .First(v => v.Id == id).PurchaseIds.Count > 0 ||
            _purchasesDao.GetAllPurchases()
                .Any(p => p.VisitorId == id) ||
            _booksDao.GetAllBooks()
                .Any(b =>
                    b.PurchasedByVisitorIds.Contains(id) ||
                    b.WishlistedByVisitorIds.Contains(id));

        if (inUse)
        {
            System.Console.WriteLine(
                "Visitor cannot be removed because purchases or books reference this visitor."
            );
            return;
        }

        _visitorsDao.RemoveVisitor(id);

        System.Console.WriteLine("Visitor removed.");
    }

    private void ShowAllVisitors()
    {
        List<Visitor> list = _visitorsDao.GetAllVisitors();

        if (list.Count == 0)
        {
            System.Console.WriteLine("No visitors found.");
            return;
        }

        foreach (Visitor v in list)
        {
            System.Console.WriteLine(
                $"ID: {v.Id} | " +
                $"Name: {v.FirstName} {v.LastName} | " +
                $"Date of birth: {v.DateOfBirth:d} | " +
                $"Address ID: {v.AddressId} | " +
                $"Phone: {v.PhoneNumber} | " +
                $"Email: {v.Email} | " +
                $"Membership card: {v.MembershipCardNumber} | " +
                $"Membership year: {v.MembershipYear} | " +
                $"Status: {v.Status} | " +
                $"Average rating: {v.AverageReviewRating} | " +
                $"Purchase IDs: {string.Join(", ", v.PurchaseIds)} | " +
                $"Wishlist book IDs: {string.Join(", ", v.WishlistBookIds)}"
            );
        }
    }
}
