using SajamKnjiga.Serialization;

namespace SajamKnjiga.Model;

public class Publisher : ISerializable
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int HeadAuthorId { get; set; }
    public List<int> AuthorIds { get; set; }
    public List<int> BookIds { get; set; }

    public Publisher()
    {
        AuthorIds = new List<int>();
        BookIds = new List<int>();
    }

    public string[] ToCSV()
    {
        return new string[]
        {
            Id.ToString(),
            Code,
            Name,
            HeadAuthorId.ToString(),
            string.Join(",", AuthorIds),
            string.Join(",", BookIds)
        };
    }

    public void FromCSV(string[] values)
    {
        Id = int.Parse(values[0]);
        Code = values[1];
        Name = values[2];
        HeadAuthorId = int.Parse(values[3]);

        AuthorIds = values[4].Length == 0
            ? new List<int>()
            : values[4].Split(',').Select(int.Parse).ToList();

        BookIds = values[5].Length == 0
            ? new List<int>()
            : values[5].Split(',').Select(int.Parse).ToList();
    }
}