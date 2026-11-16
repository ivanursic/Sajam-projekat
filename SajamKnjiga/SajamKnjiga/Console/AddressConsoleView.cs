using SajamKnjiga.DAO;
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public class AddressConsoleView
{
    private readonly AddressesDAO _addressesDAO;

    public AddressConsoleView(AddressesDAO addressesDAO)
    {
        _addressesDAO = addressesDAO;
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

            string input = System.Console.ReadLine() ?? "0";

            switch (input)
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

    private Address InputAddress()
    {
        Address address = new Address();

        System.Console.Write("Street: ");
        address.Street = System.Console.ReadLine() ?? "";

        System.Console.Write("Number: ");
        address.Number = System.Console.ReadLine() ?? "";

        System.Console.Write("City: ");
        address.City = System.Console.ReadLine() ?? "";

        System.Console.Write("Country: ");
        address.Country = System.Console.ReadLine() ?? "";

        return address;
    }

    private void AddAddress()
    {
        Address address = InputAddress();
        _addressesDAO.AddAddress(address);
        System.Console.WriteLine("Address added.");
    }

    private void UpdateAddress()
    {
        System.Console.Write("Enter address ID: ");
        int id = ConsoleViewUtils.SafeInputInt();

        Address address = InputAddress();
        address.Id = id;

        _addressesDAO.UpdateAddress(address);
        System.Console.WriteLine("Address updated if the ID exists.");
    }

    private void RemoveAddress()
    {
        System.Console.Write("Enter address ID: ");
        int id = ConsoleViewUtils.SafeInputInt();

        _addressesDAO.RemoveAddress(id);
        System.Console.WriteLine("Address removed if the ID exists.");
    }

    private void ShowAllAddresses()
    {
        List<Address> addresses = _addressesDAO.GetAllAddresses();

        if (addresses.Count == 0)
        {
            System.Console.WriteLine("No addresses found.");
            return;
        }

        foreach (Address address in addresses)
        {
            System.Console.WriteLine(
                $"ID: {address.Id}, Street: {address.Street}, " +
                $"Number: {address.Number}, City: {address.City}, " +
                $"Country: {address.Country}");
        }
    }
}
