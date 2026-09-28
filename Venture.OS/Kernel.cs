namespace Venture.OS;

/// <summary>
/// Main kernel class - inherits from Cosmos.Kernel.System.Kernel.
/// </summary>
public class Kernel : Sys.Kernel
{

    protected override void BeforeRun()
    {
        var delayInSeconds = TimeSpan.FromSeconds(0.5);
        
        Console.WriteLine($"Initializing kernel...");
        
        Thread.Sleep(delayInSeconds);
        
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
        
        Thread.Sleep(delayInSeconds);
        
        if (NetworkManager.DeviceCount > 0)
        {
            var networkInfo = $"""
                               Device:  {NetworkManager.Name}
                               MAC:     {NetworkManager.MacAddress}
                               Link up: {NetworkManager.LinkUp}
                               Ready:   {NetworkManager.Ready}
                               """;
            Console.WriteLine(networkInfo);
        }
        
        Thread.Sleep(delayInSeconds);

        DhcpClient dhcpClient = new();

        if (dhcpClient.SendDiscoverPacket() == -1) return;
        var config = NetworkManager.Primary.IPConfig;
        if (config is not null)
        {
            var dhcpInfo = $"""
                            IP address: {config.Address}
                            Subnet: {config.SubnetMask}
                            Gateway: {config.DefaultGateway}
                            """;
            Console.WriteLine(dhcpInfo);
        }
        else
            Console.WriteLine("DHCP timed out");
        
        Thread.Sleep(delayInSeconds);
        
        Console.Clear();
        
        Console.WriteLine($"{SysInfo.NAME} {SysInfo.VERSION} (Build {SysInfo.BuildNumber}) booted successfully!");
        Console.WriteLine("Type a command to get it executed.");

        /*
           var disk = StorageManager.PrimaryDevice;

                 if (disk is not null) return;
                 Console.WriteLine("No disk found.");

                 Thread.Sleep(timeDelay);

        if (Gpt.IsGpt(disk))
            Console.WriteLine($"GPT {Gpt.Parse(disk).Count} partition(s)");
        else if (Mbr.IsMbr(disk))
            Console.WriteLine($"Mbr {Mbr.Parse(disk).Count} partition(s)");
        else
            Console.WriteLine($"Unknown disk {disk}");

        Gpt.Create(disk);

        if (!PartitionManager.Create(disk, startSector: 2048, sectorCount: 131072,
                mbrSystemId: 0x0C, gptType: Gpt.BasicDataPartitionType))
        {
            Console.WriteLine("Create failed");
            return;
        }

        StorageManager.RescanPartitions(disk);

        FatFormatOptions formatOptions = new()
        {
            Type = FatType.Fat32,
            VolumeLabel = "VENTURE     "
        };

        if (StorageManager.Partitions.Count == 0
            || !VfsManager.TryFormat("fat", StorageManager.Partitions[0], formatOptions))
            Console.WriteLine("Format failed");
        */
    }

    protected override void OnBoot()
    {
        base.OnBoot();
        
        
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
