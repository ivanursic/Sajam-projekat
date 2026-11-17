
using SajamKnjiga.Model;

namespace SajamKnjiga.Console;

public static class ConsoleViewUtils
{
    public static int SafeInputInt()
    {
        while (true)
        {
            if (int.TryParse(System.Console.ReadLine(), out int result))
                return result;

            System.Console.WriteLine("Invalid number. Try again:");
        }
    }

    public static double SafeInputDouble()
    {
        while (true)
        {
            if (double.TryParse(
                System.Console.ReadLine(),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out double result) && double.IsFinite(result))
            {
                return result;
            }

            System.Console.WriteLine(
                "Invalid number. Use a dot for decimals (for example 12.50). Try again:"
            );
        }
    }

    public static double SafeInputAverageRating()
    {
        while (true)
        {
            double rating = SafeInputDouble();

            if (rating >= 0 && rating <= 5)
                return rating;

            System.Console.WriteLine(
                "Average review rating must be between 0 and 5. Try again:"
            );
        }
    }

    public static DateTime SafeInputDate()
    {
        while (true)
        {
            if (DateTime.TryParse(
                System.Console.ReadLine(),
                out DateTime result))
            {
                return result;
            }

            System.Console.WriteLine("Invalid date. Try again:");
        }
    }

    public static int ReadExistingId(
        IEnumerable<int> availableIds,
        string entityName)
    {
        HashSet<int> ids = availableIds.ToHashSet();

        while (true)
        {
            int id = SafeInputInt();

            if (ids.Contains(id))
                return id;

            System.Console.WriteLine(
                $"No {entityName} with ID {id} exists. Enter an existing {entityName} ID:"
            );
        }
    }

    public static List<int> ReadExistingIds(
        IEnumerable<int> availableIds,
        string entityName)
    {
        HashSet<int> validIds = availableIds.ToHashSet();

        while (true)
        {
            string input = System.Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(input))
                return new List<int>();

            string[] values = input.Split(',');
            List<int> ids = new List<int>();
            bool valid = true;

            foreach (string value in values)
            {
                if (!int.TryParse(value.Trim(), out int id) ||
                    !validIds.Contains(id) ||
                    ids.Contains(id))
                {
                    valid = false;
                    break;
                }

                ids.Add(id);
            }

            if (valid)
                return ids;

            System.Console.WriteLine(
                $"Invalid or duplicate {entityName} ID. Enter existing IDs separated by commas, or leave empty:"
            );
        }
    }

    public static VisitorStatus ReadVisitorStatus()
    {
        while (true)
        {
            string input = System.Console.ReadLine() ?? string.Empty;

            if (Enum.TryParse(input, true, out VisitorStatus status) &&
                Enum.IsDefined(typeof(VisitorStatus), status))
            {
                return status;
            }

            System.Console.WriteLine("Enter Regular or VIP:");
        }
    }
}
