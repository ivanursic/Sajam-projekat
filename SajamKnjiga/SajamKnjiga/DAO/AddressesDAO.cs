using SajamKnjiga.Model;
using SajamKnjiga.Storage;

namespace SajamKnjiga.DAO;

public class AddressesDAO
{
    private readonly Storage<Address> _storage = new("addresses.txt");
    private readonly List<Address> _addresses;

    public AddressesDAO()
    {
        _addresses = _storage.Load();
    }

    private int GenerateId()
    {
        return _addresses.Count == 0 ? 0 : _addresses.Max(a => a.Id) + 1;
    }

    public void AddAddress(Address address)
    {
        address.Id = GenerateId();
        _addresses.Add(address);
        _storage.Save(_addresses);
    }

    public void UpdateAddress(Address address)
    {
        Address? oldAddress = GetAddressById(address.Id);

        if (oldAddress == null) return;

        oldAddress.Street = address.Street;
        oldAddress.Number = address.Number;
        oldAddress.City = address.City;
        oldAddress.Country = address.Country;

        _storage.Save(_addresses);
    }

    public void RemoveAddress(int id)
    {
        Address? address = GetAddressById(id);

        if (address == null) return;

        _addresses.Remove(address);
        _storage.Save(_addresses);
    }

    private Address? GetAddressById(int id)
    {
        return _addresses.FirstOrDefault(a => a.Id == id);
    }

    public List<Address> GetAllAddresses()
    {
        return _addresses;
    }
}
