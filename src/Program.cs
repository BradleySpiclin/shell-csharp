namespace src;

public class Program
{
    public static readonly HashSet<string> ValidCommands =
    [
        Exit,
        Echo,
        Type
    ];

    public const string Exit = "exit";
    public const string Echo = "echo";
    public const string Type = "type";

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
            else if (parts[0] == Type)
            {
                if (ValidCommands.Contains(parts[1]))
                {
                    Console.WriteLine($"{parts[1]} is a shell builtin");
                }
                else
                {
                    Console.WriteLine($"{string.Join(" ", parts.Skip(1))}: not found");
                }
            }
        }
    }
}