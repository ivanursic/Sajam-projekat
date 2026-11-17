using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class AddressConsoleView
{
    private readonly AddressesDAO _addressesDAO;
    private readonly VisitorsDAO _visitorsDAO;
    private readonly AuthorsDAO _authorsDAO;

    public AddressConsoleView(AddressesDAO addressesDAO, VisitorsDAO visitorsDAO, AuthorsDAO authorsDAO)
    {
        _addressesDAO = addressesDAO;
        _visitorsDAO = visitorsDAO;
        _authorsDAO = authorsDAO;
    }

    public void RunMenu()
    {
        while (true)
        {
            System.Console.WriteLine("\n=== ADDRESSES ===");
            System.Console.WriteLine("1: Show all addresses");
            System.Console.WriteLine("2: Add address");
            System.Console.WriteLine("3: Update address");
            System.Console.WriteLine("4: Remove address");
            System.Console.WriteLine("0: Back");

            switch (System.Console.ReadLine())
            {
                case "1": ShowAllAddresses(); break;
                case "2": AddAddress(); break;
                case "3": UpdateAddress(); break;
                case "4": RemoveAddress(); break;
                case "0": return;
                default: System.Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private Address InputAddress() => new Address
    {
        Street = ReadText("Street: "),
        Number = ReadText("Number: "),
        City = ReadText("City: "),
        Country = ReadText("Country: ")
    };

    private static string ReadText(string prompt)
    {
        System.Console.Write(prompt);
        return System.Console.ReadLine() ?? string.Empty;
    }

    private void AddAddress()
    {
        _addressesDAO.AddAddress(InputAddress());
        System.Console.WriteLine("Address added.");
    }

    private void UpdateAddress()
    {
        System.Console.Write("Enter address ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_addressesDAO.GetAllAddresses().Select(a => a.Id), "address");
        Address address = InputAddress();
        address.Id = id;
        _addressesDAO.UpdateAddress(address);
        System.Console.WriteLine("Address updated.");
    }

    private void RemoveAddress()
    {
        System.Console.Write("Enter address ID: ");
        int id = ConsoleViewUtils.ReadExistingId(_addressesDAO.GetAllAddresses().Select(a => a.Id), "address");

        bool inUse = _visitorsDAO.GetAllVisitors().Any(v => v.AddressId == id) ||
                     _authorsDAO.GetAllAuthors().Any(a => a.AddressId == id);
        if (inUse)
        {
            System.Console.WriteLine("Address cannot be removed because a visitor or author uses it.");
            return;
        }

        _addressesDAO.RemoveAddress(id);
        System.Console.WriteLine("Address removed.");
    }

    private void ShowAllAddresses()
    {
        List<Address> addresses = _addressesDAO.GetAllAddresses();
        if (addresses.Count == 0) { System.Console.WriteLine("No addresses found."); return; }

        foreach (Address a in addresses)
            System.Console.WriteLine($"ID: {a.Id}, Street: {a.Street}, Number: {a.Number}, City: {a.City}, Country: {a.Country}");
    }
}
