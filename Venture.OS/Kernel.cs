using Venture.OS.Startup;

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
        Bootstrap.BootIntoRam();
        Thread.Sleep(delayInSeconds);
        Bootstrap.IsTimeManagerEnabled();
        Thread.Sleep(delayInSeconds);
        Bootstrap.ConnectToNetwork();
        Thread.Sleep(delayInSeconds);
        
        Console.Clear();
        
        Console.WriteLine($"{HostInfo.Name} {HostInfo.Version} (Build {HostInfo.BuildNumber}) booted successfully!");
        Console.WriteLine("Type a command to get it executed.");
    }
    
    // Early initialization
    protected override void OnBoot()
    {
        base.OnBoot();   // keep the KernelConsole setup; drop this line to boot headless

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
