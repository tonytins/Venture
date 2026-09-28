namespace Venture.OS;

public static class Bootstrap
{
    public static void BootIntoRam()
    {
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
    }

    public static void ConnectToNetwork()
    {
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
    }
}