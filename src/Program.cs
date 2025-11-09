public class Program
{
    private static readonly HashSet<string> Commands =
    [
    ];

    static void Main()
    {
        Console.Write("$ ");
        var input = Console.ReadLine();

        if (!Commands.Contains(input ?? string.Empty))
        {
            Console.WriteLine("command not found");
        }
    }
}