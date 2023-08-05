using FlexLauncher.Core;
using FlexLauncher.Data;
using FlexLauncher.Parsing;
using FlexLauncher.Util;

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
        
        // For some reason it still launches fine without an access token.
        // Minecraft doesn't complain about it at all.
        // 
        // Legal mumbo jumbo:
        // We don't want to get sued by Mojang or Microsoft for this, if this gets into production.
        // As launching without a token is piracy.
        // TODO: Use an empty token in debug only.
        // TODO: Prompt the user to login and properly check game ownership using Mojang API in production.
        //
        // DO NOT WORK ON THIS CODE IF YOU DO NOT HAVE A MINECRAFT ACCOUNT.
        // DO NOT USE THIS PRE-ALPHA CODE FOR THE PURPOSE OF PLAYING MINECRAFT FOR FREE. 
        // IF YOU DO NOT OWN A MINECRAFT ACCOUNT, PLEASE CLOSE YOUR CODE EDITOR AND CEASE ALL INTERACTIONS WITH THIS CODEBASE IMMEDIATELY.
        // WE SHALL NOT BE HELD LIABLE FOR ANY DAMAGES OR MISUSE CAUSED BY THIS TEMPORARY WORKAROUND.
        // YOU HAVE BEEN WARNED.
        var authInfo = new AuthInfo("Reimnop", "2d4faffa-8e09-4627-9fdd-a0eddc2fc981", "", "", UserType.Microsoft);
        
        var context = new LaunchContext(versionInfo, authInfo, preferences);
        var task = LaunchMinecraft(context);
        task.Wait();
    }

    private static async Task LaunchMinecraft(LaunchContext context)
    {
        var launcher = new Launcher(context);

        var progress = new Progress<DownloadProgress>(x =>
        {
            Console.WriteLine($"Downloaded '{Path.GetFileName(x.LastDownloadedFile)}', {x.DownloadedFiles}/{x.TotalFiles}");
        });
        await launcher.InstallVersionAsync(progress);
        
        var gameInstance = launcher.GetGameInstance();
        var process = gameInstance.Process;
        process.StartInfo.UseShellExecute = true;
        process.Start();
        
        await process.WaitForExitAsync();
    }
}