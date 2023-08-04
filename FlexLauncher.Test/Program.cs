using FlexLauncher.Core;
using FlexLauncher.Data;
using FlexLauncher.Parsing;

namespace FlexLauncher.Test;

public class Program
{
    public static void Main(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var workingPath = Path.Combine(basePath, "game");
        var gamePath = Path.Combine(basePath, "instance");

        var versionInfo = VersionInfoParser.FromFile("23w31a.json");
        var preferences = new Preferences(
            "FlexLauncher", 
            "21", 
            "", 
            @"C:\Program Files\Eclipse Adoptium\jre-17.0.1.12-hotspot\bin\java.exe", 
            workingPath,
            gamePath, 
            false, 
            null,
            null);
        var authInfo = new AuthInfo("Reimnop", "2d4faffa-8e09-4627-9fdd-a0eddc2fc981", "", "", UserType.Microsoft);
        
        var context = new LaunchContext(versionInfo, authInfo, preferences);
        var task = LaunchMinecraft(context);
        task.Wait();
    }

    private static async Task LaunchMinecraft(LaunchContext context)
    {
        var launcher = new Launcher(context);
        await launcher.InstallVersionAsync();
        
        var gameInstance = launcher.GetGameInstance();
        var process = gameInstance.Process;
        process.StartInfo.UseShellExecute = true;
        process.Start();
        
        await process.WaitForExitAsync();
    }
}