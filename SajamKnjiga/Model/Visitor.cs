using System.Globalization;
using SajamKnjiga.Serialization;

namespace SajamKnjiga.Model;

public class Visitor : ISerializable
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int AddressId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MembershipCardNumber { get; set; } = string.Empty;
    public int MembershipYear { get; set; }
    public VisitorStatus Status { get; set; }
    public double AverageReviewRating { get; set; }
    public List<int> PurchaseIds { get; set; }
    public List<int> WishlistBookIds { get; set; }

    public Visitor()
    {
        PurchaseIds = new List<int>();
        WishlistBookIds = new List<int>();
    }

    public string[] ToCSV()
    {
        return new string[]
        {
            Id.ToString(),
            FirstName,
            LastName,
            DateOfBirth.ToString("O"),
            AddressId.ToString(),
            PhoneNumber,
            Email,
            MembershipCardNumber,
            MembershipYear.ToString(),
            Status.ToString(),
            AverageReviewRating.ToString(CultureInfo.InvariantCulture),
            string.Join(",", PurchaseIds),
            string.Join(",", WishlistBookIds)
        };
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        FirstName = values[1];
        LastName = values[2];
        DateOfBirth = DateTime.Parse(values[3], null, DateTimeStyles.RoundtripKind);
        AddressId = int.Parse(values[4]);
        PhoneNumber = values[5];
        Email = values[6];
        MembershipCardNumber = values[7];
        MembershipYear = int.Parse(values[8]);
        Status = Enum.Parse<VisitorStatus>(values[9]);
        AverageReviewRating = double.Parse(values[10], CultureInfo.InvariantCulture);
        PurchaseIds = values[11].Length == 0
            ? new List<int>()
            : values[11].Split(',').Select(int.Parse).ToList();
        WishlistBookIds = values[12].Length == 0
            ? new List<int>()
            : values[12].Split(',').Select(int.Parse).ToList();
    }
}

public enum VisitorStatus
{
    Regular,
    VIP
}