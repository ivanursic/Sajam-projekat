using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class VisitorConsoleView
{
    private readonly VisitorsDAO _visitorsDao;

    public VisitorConsoleView(VisitorsDAO visitorsDao)
    {
        _visitorsDao = visitorsDao;
    }

    public void RunMenu()
    {
        while (true)
        {
            ShowMenu();
            string input = System.Console.ReadLine() ?? "0";

            if (input == "0") break;

            HandleMenuInput(input);
        }
    }

    private void ShowMenu()
    {
        System.Console.WriteLine("\n--- Visitors ---");
        System.Console.WriteLine("1: Show all visitors");
        System.Console.WriteLine("2: Add visitor");
        System.Console.WriteLine("3: Update visitor");
        System.Console.WriteLine("4: Remove visitor");
        System.Console.WriteLine("0: Back");
    }

    private void HandleMenuInput(string input)
    {
        switch (input)
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
            default:
                System.Console.WriteLine("Invalid option.");
                break;
        }
    }

    private void ShowAllVisitors()
    {
        List<Visitor> visitors = _visitorsDao.GetAllVisitors();

        System.Console.WriteLine("\nVisitors:");

        foreach (Visitor visitor in visitors)
        {
            System.Console.WriteLine(
                $"ID: {visitor.Id} | " +
                $"Name: {visitor.FirstName} {visitor.LastName} | " +
                $"Date of birth: {visitor.DateOfBirth:d} | " +
                $"Address ID: {visitor.AddressId} | " +
                $"Phone: {visitor.PhoneNumber} | " +
                $"Email: {visitor.Email} | " +
                $"Membership card: {visitor.MembershipCardNumber} | " +
                $"Membership year: {visitor.MembershipYear} | " +
                $"Status: {visitor.Status} | " +
                $"Average rating: {visitor.AverageReviewRating}"
            );
        }
    }

    private void AddVisitor()
    {
        Visitor visitor = InputVisitor();
        _visitorsDao.AddVisitor(visitor);
        System.Console.WriteLine("Visitor added.");
    }

    private Visitor InputVisitor()
    {
        Visitor visitor = new Visitor();

        System.Console.WriteLine("Enter first name:");
        visitor.FirstName = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Enter last name:");
        visitor.LastName = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Enter date of birth:");
        visitor.DateOfBirth = ConsoleViewUtils.SafeInputDate();

        System.Console.WriteLine("Enter address ID:");
        visitor.AddressId = ConsoleViewUtils.SafeInputInt();

        System.Console.WriteLine("Enter phone number:");
        visitor.PhoneNumber = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Enter email:");
        visitor.Email = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Enter membership card number:");
        visitor.MembershipCardNumber = System.Console.ReadLine() ?? string.Empty;

        System.Console.WriteLine("Enter membership year:");
        visitor.MembershipYear = ConsoleViewUtils.SafeInputInt();

        System.Console.WriteLine("Enter status (Regular/VIP):");
        string status = System.Console.ReadLine() ?? "Regular";
        visitor.Status = Enum.Parse<VisitorStatus>(status, true);

        System.Console.WriteLine("Enter average review rating:");
        visitor.AverageReviewRating = ConsoleViewUtils.SafeInputDouble();

        return visitor;
    }

    private void UpdateVisitor()
    {
        int id = InputId();
        Visitor visitor = InputVisitor();
        visitor.Id = id;

        Visitor? updatedVisitor = _visitorsDao.UpdateVisitor(visitor);

        if (updatedVisitor == null)
        {
            System.Console.WriteLine("Visitor not found.");
            return;
        }

        System.Console.WriteLine("Visitor updated.");
    }

    private int InputId()
    {
        System.Console.WriteLine("Enter visitor ID:");
        return ConsoleViewUtils.SafeInputInt();
    }

    private void RemoveVisitor()
    {
        int id = InputId();
        Visitor? removedVisitor = _visitorsDao.RemoveVisitor(id);

        if (removedVisitor == null)
        {
            System.Console.WriteLine("Visitor not found.");
            return;
        }

        System.Console.WriteLine("Visitor removed.");
    }
}
