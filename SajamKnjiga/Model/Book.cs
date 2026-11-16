using System.Globalization;
using SajamKnjiga.Serialization;

namespace SajamKnjiga.Model;

public class Book : ISerializable
{
    public int Id { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public double Price { get; set; }
    public int PageCount { get; set; }

    public List<int> AuthorIds { get; set; }
    public int PublisherId { get; set; }
    public List<int> PurchasedByVisitorIds { get; set; }
    public List<int> WishlistedByVisitorIds { get; set; }

    public Book()
    {
        AuthorIds = new List<int>();
        PurchasedByVisitorIds = new List<int>();
        WishlistedByVisitorIds = new List<int>();
    }

    public string[] ToCSV()
    {
        return new string[]
        {
            Id.ToString(),
            ISBN,
            Title,
            Genre,
            PublicationYear.ToString(),
            Price.ToString(CultureInfo.InvariantCulture),
            PageCount.ToString(),
            string.Join(",", AuthorIds),
            PublisherId.ToString(),
            string.Join(",", PurchasedByVisitorIds),
            string.Join(",", WishlistedByVisitorIds)
        };
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        ISBN = values[1];
        Title = values[2];
        Genre = values[3];
        PublicationYear = int.Parse(values[4]);
        Price = double.Parse(values[5], CultureInfo.InvariantCulture);
        PageCount = int.Parse(values[6]);

        AuthorIds = values[7].Length > 0
            ? values[7].Split(',').Select(int.Parse).ToList()
            : new List<int>();

        PublisherId = int.Parse(values[8]);

        PurchasedByVisitorIds = values[9].Length > 0
            ? values[9].Split(',').Select(int.Parse).ToList()
            : new List<int>();

        WishlistedByVisitorIds = values[10].Length > 0
            ? values[10].Split(',').Select(int.Parse).ToList()
            : new List<int>();
    }
}
