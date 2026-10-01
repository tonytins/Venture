namespace Venture.OS.Startup;

internal sealed class BootRamDisk(string name, ulong blockSize, ulong blockCount) : IBlockDevice
{
    private readonly byte[] storage = new byte[blockCount * blockSize];

    public string Name { get; } = name;
    public ulong BlockSize { get; } = blockSize;
    public ulong BlockCount { get; } = blockCount;

    void IBlockDevice.ReadBlock(ulong blockNo, ulong blockCount, Span<byte> data) => storage.AsSpan((int)(blockNo * BlockSize), (int)(blockCount * BlockSize)).CopyTo(data);

    void IBlockDevice.WriteBlock(ulong blockNo, ulong blockCount, ReadOnlySpan<byte> data) => data[..(int)(blockCount * BlockSize)].CopyTo(storage.AsSpan((int)(blockNo * BlockSize)));

    void IBlockDevice.Flush() { }
}
