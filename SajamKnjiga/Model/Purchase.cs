using System.Globalization;
using SajamKnjiga.Serialization;

namespace SajamKnjiga.Model;

public class Purchase : ISerializable
{
    public int Id { get; set; }
    public int VisitorId { get; set; }
    public int BookId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public string[] ToCSV()
    {
        return new string[]
        {
            Id.ToString(),
            VisitorId.ToString(),
            BookId.ToString(),
            PurchaseDate.ToString("O"),
            Rating.ToString(),
            Comment
        };
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        VisitorId = int.Parse(values[1]);
        BookId = int.Parse(values[2]);
        PurchaseDate = DateTime.Parse(
            values[3],
            null,
            DateTimeStyles.RoundtripKind
        );
        Rating = int.Parse(values[4]);
        Comment = values[5];
    }
}