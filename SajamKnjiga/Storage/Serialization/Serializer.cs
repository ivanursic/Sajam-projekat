using System.Text;

namespace SajamKnjiga.Serialization;

public class Serializer<T> where T : ISerializable, new()
{
    private const char Delimiter = '|';

    public string ToCSV(List<T> objects)
    {
        StringBuilder csv = new StringBuilder();

        foreach (T obj in objects)
        {
            csv.AppendLine(string.Join(Delimiter, obj.ToCSV()));
        }

        return csv.ToString();
    }

    public List<T> FromCSV(IEnumerable<string> lines)
    {
        List<T> objects = new List<T>();

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] values = line.Split(Delimiter);

            T obj = new T();
            obj.FromCSV(values);

            objects.Add(obj);
        }

        return objects;
    }
}
