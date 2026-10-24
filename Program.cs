using System.Security.Cryptography;
using System.Text;
using System.Runtime.Versioning;
using Microsoft.Extensions.Configuration;
 
// DPAPI works on Windows only.
[assembly: SupportedOSPlatform("windows")]
 
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();
 
string encrypted = config["EncryptedPassword"] ?? "";
 
if (string.IsNullOrEmpty(encrypted))
{
    // RUN 1: one-time setup. Lock a demo password and print the locked text.
    string demoPassword = "DemoPassword123!";   // fake password for the demo only
 
    byte[] locked = ProtectedData.Protect(
        Encoding.UTF8.GetBytes(demoPassword),
        null,
        DataProtectionScope.CurrentUser);
 
    Console.WriteLine("EncryptedPassword is empty, so this is the one-time setup.");
    Console.WriteLine("Copy the text below into appsettings.json as the EncryptedPassword value:");
    Console.WriteLine();
    Console.WriteLine(Convert.ToBase64String(locked));
}
else
{
    // RUN 2 and every run after: read the locked text and unlock it.
    byte[] unlocked = ProtectedData.Unprotect(
        Convert.FromBase64String(encrypted),
        null,
        DataProtectionScope.CurrentUser);
 
    Console.WriteLine("Found a locked password in appsettings.json. Unlocking it with DPAPI...");
    // Printing a real password is for the demo only. A real app would use it silently.
    Console.WriteLine("Decrypted password: " + Encoding.UTF8.GetString(unlocked));
}
 