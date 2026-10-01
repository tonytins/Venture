namespace Venture.OS.Startup;

internal static class Bootstrap
{
    public static void BootIntoRam()
    {
        Console.WriteLine("Initializing filesystem...");

        // RAM disk
        BootRamDisk ramdisk = new("RAMDISK", 512, 65536); // 32 MiB
        FatFilesystemType fat = new(ramdisk);

        if (!VfsManager.RegisterFilesystem("ramfat", fat)
            || !VfsManager.TryFormat("ramfat", "", new FatFormatOptions { Type = FatType.Fat16 })
            || !VfsManager.TryMount("ramfat", "", MountFlags.None, "/mnt", out _))
        {
            Console.WriteLine("RAM disk setup failed.");
        }
    }

    public static void IsTimeManagerEnabled()
    {
        Console.WriteLine("Initializing time manager...");
        if (!TimerManager.IsInitialized)
            Console.WriteLine("Time manager setup failed.");
    }

    public static void ConnectToNetwork()
    {
        Console.WriteLine("Initializing network...");

        if (HostInfo.IsLinkedToNetwork)
        {
            var networkInfo = $"""
                               Device:  {NetworkManager.Name}
                               MAC:     {NetworkManager.MacAddress}
                               Link up: {NetworkManager.LinkUp}
                               Ready:   {NetworkManager.Ready}
                               """;
            HostInfo.MacAddress = NetworkManager.MacAddress?.ToString();
            HostInfo.Domain = NetworkManager.Name;
            Console.WriteLine(networkInfo);
        }

        DhcpClient dhcpClient = new();

        if (dhcpClient.SendDiscoverPacket() == -1) return;
        if (HostInfo.HasIp)
        {
            var config = NetworkManager.Primary.IPConfig;
            var dhcpInfo = $"""
                            IP address: {config?.Address}
                            Subnet: {config?.SubnetMask}
                            Gateway: {config?.DefaultGateway}
                            """;
            HostInfo.IpAddress = config?.Address.ToString();
            Console.WriteLine(dhcpInfo);
        }
        else
            Console.WriteLine("DHCP timed out");
    }
}
