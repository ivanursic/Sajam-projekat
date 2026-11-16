using System.Globalization;
using SajamKnjiga.Serialization;

namespace SajamKnjiga.Model;

public class Author : ISerializable
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int AddressId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string IdentityCardNumber { get; set; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public List<int> BookIds { get; set; }

    public Author()
    {
        BookIds = new List<int>();
    }

    public string[] ToCSV()
    {
        string[] values =
        {
            Id.ToString(),
            FirstName,
            LastName,
            DateOfBirth.ToString("o", CultureInfo.InvariantCulture),
            AddressId.ToString(),
            PhoneNumber,
            Email,
            IdentityCardNumber,
            YearsOfExperience.ToString(),
            string.Join(",", BookIds)
        };

        return values;
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        FirstName = values[1];
        LastName = values[2];
        DateOfBirth = DateTime.Parse(values[3], CultureInfo.InvariantCulture);
        AddressId = int.Parse(values[4]);
        PhoneNumber = values[5];
        Email = values[6];
        IdentityCardNumber = values[7];
        YearsOfExperience = int.Parse(values[8]);

        BookIds = values[9].Length > 0
            ? values[9].Split(',').Select(int.Parse).ToList()
            : new List<int>();
    }
}
