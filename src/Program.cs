using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using Mono.Unix.Native;

namespace src;

public class Program
{
    public static readonly HashSet<string> BuiltinCommands =
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

            if (!BuiltinCommands.Contains(parts?[0] ?? string.Empty))
            {
                Console.WriteLine($"{input}: command not found");
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
                if (BuiltinCommands.Contains(parts[1]))
                {

                    Console.WriteLine($"{parts[1]} is a shell builtin");
                }
                else if (TryGetExecutable(parts[1], out var path))
                {
                    Console.WriteLine($"{parts[0]} is {path}");
                }
                else
                {
                    Console.WriteLine($"{string.Join(" ", parts.Skip(1))}: not found");
                }
            }
        }
    }

    public static bool TryGetExecutable(string command, out string? path)
    {
        path = null;
        Console.WriteLine($"Command: {command}");
        var pathVariable = Environment.GetEnvironmentVariable("PATH");
        if (!string.IsNullOrEmpty(pathVariable))
        {
            var pathDirectories = pathVariable.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var directory in pathDirectories)
            {
                if (!Directory.Exists(directory))
                    continue;

                foreach (var file in Directory.EnumerateFiles(directory))
                {
                    Console.WriteLine($"FILE: {file}");
                    Console.WriteLine($"COMMAND: {command}");
                    var fileFromPath = file.Split('/').Last();
                    Console.WriteLine($"FILE FROM PATH: {fileFromPath}");
                    if (fileFromPath == command && IsExecutable(file))
                    {
                        path = file;
                        return true;
                    }
                }
            }
        }

        static bool IsExecutable(string filePath)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var ext = Path.GetExtension(filePath).ToUpperInvariant();

                var pathExt = Environment.GetEnvironmentVariable("PATHEXT")?
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(e => e.ToUpperInvariant())
                    .ToHashSet()
                    ?? new HashSet<string>();

                return pathExt.Contains(ext);
            }
            else
            {
                var fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                    return false;

                try
                {
                    var mode = Syscall.stat(filePath, out var stat);
                    if (mode == 0)
                    {
                        return (stat.st_mode &
                            (FilePermissions.S_IXUSR |
                             FilePermissions.S_IXGRP |
                             FilePermissions.S_IXOTH)) != 0;
                    }
                }
                catch { }

                return false;
            }
        }

        return false;
    }
}