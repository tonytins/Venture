namespace Venture.OS;

internal struct HostInfo
{
    /// <summary>
    /// The name of the operating system.
    /// </summary>
    public const string Name = "VentureOS";

    /// <summary>
    /// The version of the operating system using the ThisAssembly.Git.SemVer object.
    /// </summary>
    public const string Version = $"{ThisAssembly.Git.SemVer.Major}.{ThisAssembly.Git.SemVer.Minor}.{ThisAssembly.Git.SemVer.Patch}";

    public static string? IpAddress { get; set; } = string.Empty;

    public static string? MacAddress { get; set; } = string.Empty;

    /// <summary>
    ///  Host's name given by the user.
    /// </summary>
    public static string? Domain { get; set; } = string.Empty;

    public static bool IsLinkedToNetwork => NetworkManager.DeviceCount > 0;

    public static bool HasIp => NetworkManager.Primary.IPConfig is not null;


    /// <summary>
    /// Generates the build number from the commit hash.
    /// </summary>
    /// <returns>The build number as a uint.</returns>
    public static uint BuildNumber
    {
        get
        {
            // Get the bytes of the commit hash as a UTF-8 encoded string
            var commit = Encoding.UTF8.GetBytes(ThisAssembly.Git.Commit);

            // Convert the first 4 bytes of the commit hash to a uint and return it modulo 1000000
            // (this will give us a 6-digit number with the first 3 digits being the first 3 digits of the commit hash
            // and the last 3 digits being the last 3 digits of the commit hash)
            return BitConverter.ToUInt32(commit, 0) % 1000000;
        }
    }
}
