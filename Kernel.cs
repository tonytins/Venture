namespace Venture;

/// <summary>
/// Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
/// </summary>
public class Kernel : Sys.Kernel
{
    protected override void BeforeRun()
    {
        Console.WriteLine($"{SysInfo.NAME} {SysInfo.VERSION} (Build {SysInfo.BuildNumber}) booted successfully!");
        Console.WriteLine("Type a command to get it executed.");
    }

    protected override void Run()
    {
        Console.Write("> ");
        var input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
            return;

        switch (input.ToLower())
        {
            case "help":
                var help = """
            Available commands:
                help     - Show this help message
                clear    - Clear the screen
                halt     - Halt the system
            """;
                Console.WriteLine(help);
                break;

            case "clear":
                Console.Clear();
                break;

            case "halt":
                Console.WriteLine("Halting system...");
                Stop();
                break;

            default:
                Console.WriteLine($"\"{input}\" is not a command");
                break;
        }
    }

    public override void BeforeRun()
    {
        base.BeforeRun();
        Console.WriteLine($"Initializing {SysInfo.NAME} kernel...");

        // File system initialization
        FatFileSystem fat = new();

        if (!VfsManager.RegisterFileSystem("fat", fat))
        {
            Console.WriteLine("Failed to register FAT file system.");
            return;
        }

        if (StorageManager.Partitions.Count == 0)
        {
            Console.WriteLine("No partitions found.");
            return;
        }

        if (VfsManager.TryMount("fat", StorageManager.Partitions[0], MountFlags.None, "/mnt", out VfsManager.VfsMount? mount))
        {
            Console.WriteLine("Mounted " + mount.Name + " at " + mount.MountPoint);
        }
    }
}
