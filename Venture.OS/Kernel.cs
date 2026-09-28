namespace Venture.OS;

/// <summary>
/// Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
/// </summary>
public class Kernel : Sys.Kernel
{

    protected override void BeforeRun()
    {
        Console.WriteLine($"Initializing {SysInfo.NAME} kernel...");
        
        // RAM disk
        BootRamDisk ramdisk = new("RAMDISK", 512, 65536); // 32 MiB
        FatFilesystemType fat = new(ramdisk);
        
        if (!VfsManager.RegisterFilesystem("ramfat", fat)
            || !VfsManager.TryFormat("ramfat", "", new FatFormatOptions { Type = FatType.Fat16 })
            || !VfsManager.TryMount("ramfat", "", MountFlags.None, "/mnt", out _))
        {
            Console.WriteLine("RAM disk setup failed.");
            return;
        }

        // File system initialization
        // TODO: Move to separate class
        /*
        if (!VfsManager.RegisterFilesystem("fat", fat))
        {
            Console.WriteLine("Failed to register FAT file system.");
            return;
        }

        if (StorageManager.Partitions.Count == 0)
        {
            Console.WriteLine("No partitions found.");
            return;
        }

        if (VfsManager.TryMount("fat", StorageManager.Partitions[0], MountFlags.None, "/mnt", out var mount))
        {
            Console.WriteLine($"Mounted {mount.Name} at {mount.MountPoint}");
        } */

        if (NetworkManager.DeviceCount > 0)
        {
            const string networkDevices = """
                                          Device: {NetworkManager.Name}
                                          MAC: {NetworkManager.MacAddress}
                                          Link up: {NetworkManager.LinkUp}
                                          Ready: {NetworkManager.Ready}
                                          """;
            Console.WriteLine(networkDevices);
        }

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
                const string help = """
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
}
