public class Program
{
    // This is fine for now we'll see how it grows over time
    public static readonly HashSet<string> ValidCommands =
    [
        Exit,
        Echo
    ];

    public const string Exit = "exit";
    public const string Echo = "echo";

    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            var input = Console.ReadLine();
            var parts = input?.Split(' ').ToArray();

            if (!ValidCommands.Contains(parts?[0] ?? string.Empty))
            {
                Console.WriteLine($"{input}: command not found");
                continue;
            }

            // Bit of a hack lets parse the command properly when I have time
            if (parts[0] == Exit)
            {
                // if (parts.Length > 1)
                // {
                //     Console.WriteLine($"{Exit} {parts[1]}");
                //     Environment.Exit(int.Parse(parts[1]));
                // }

                // Console.WriteLine($"{Exit}");
                Environment.Exit(0);
            }

            if (parts[0] == Echo)
            {
                Console.WriteLine($"{string.Join(" ", parts.Skip(1))}");
            }
        }
    }
}