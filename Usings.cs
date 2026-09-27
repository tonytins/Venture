global using System.IO;
global using System.Text;
global using System.Net;
global using System.Net.Sockets;

// Cosmos kernel
global using Sys = Cosmos.Kernel.System;

// Filesystem and storage
global using Cosmos.Kernel.System.Storage;
global using Cosmos.Kernel.System.Vfs;
global using Cosmos.Kernel.System.Filesystems.Fat;
global using Cosmos.Kernel.HAL.Interfaces.Devices;
global using Cosmos.Kernel.HAL.Vfs;

// Networking
global using Cosmos.Kernel.System.Network;
global using Cosmos.Kernel.System.Network.Config;
global using Cosmos.Kernel.System.Network.DNS;
global using Cosmos.Kernel.System.Network.IPv4;
global using Cosmos.Kernel.System.Network.IPv4.DHCP;
global using Cosmos.Kernel.System.Timer;