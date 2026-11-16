namespace SajamKnjiga.Console;

public static class ConsoleViewUtils
{
    public static int SafeInputInt()
    {
        int input;
        string rawInput = System.Console.ReadLine() ?? string.Empty;

        while (!int.TryParse(rawInput, out input))
        {
            System.Console.WriteLine("Not a valid number, try again:");
            rawInput = System.Console.ReadLine() ?? string.Empty;
        }

        return input;
    }

    public static double SafeInputDouble()
    {
        double input;
        string rawInput = System.Console.ReadLine() ?? string.Empty;

        while (!double.TryParse(rawInput, out input))
        {
            System.Console.WriteLine("Not a valid number, try again:");
            rawInput = System.Console.ReadLine() ?? string.Empty;
        }

        return input;
    }

    public static DateTime SafeInputDate()
    {
        DateTime input;
        string rawInput = System.Console.ReadLine() ?? string.Empty;

        while (!DateTime.TryParse(rawInput, out input))
        {
            System.Console.WriteLine("Not a valid date, try again:");
            rawInput = System.Console.ReadLine() ?? string.Empty;
        }

        return input;
    }
}