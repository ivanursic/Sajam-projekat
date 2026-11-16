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
            if (double.TryParse(System.Console.ReadLine(), out double result))
                return result;

            System.Console.WriteLine("Invalid number. Try again:");
        }
    }

    public static DateTime SafeInputDate()
    {
        while (true)
        {
            if (DateTime.TryParse(System.Console.ReadLine(), out DateTime result))
                return result;

            System.Console.WriteLine("Invalid date. Try again:");
        }
    }

    public static List<int> SafeInputIds()
    {
        while (true)
        {
            string input = System.Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(input))
                return new List<int>();

            string[] values = input.Split(',');

            try
            {
                return values.Select(int.Parse).ToList();
            }
            catch
            {
                System.Console.WriteLine("Enter IDs separated by commas:");
            }
        }
    }
}
