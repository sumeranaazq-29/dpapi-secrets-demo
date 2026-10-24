# Protecting Secrets with Windows DPAPI (C# Demo)

A small .NET console app that shows how the Windows Data Protection API (DPAPI) can keep a password out of a config file in plain text. The app locks a password once, saves only the locked text, and unlocks it again each time it starts.

![First run](docs/screenshots/run1-encrypt.png)
<!-- Add your screenshot of run 1 here -->

## The problem

Applications often keep database passwords in `appsettings.json`. If that file is copied, leaked or committed to a repository, the password is exposed. DPAPI lets an application store an encrypted value instead, which only the same Windows user account on the same machine can decrypt.

## How it works

| Run | What happens |
| --- | --- |
| Run 1, one-time setup | `EncryptedPassword` is empty, so the app locks a demo password with DPAPI and prints the locked text. You paste that text into `appsettings.json`. |
| Run 2 and every run after | The app finds the locked text in the file, unlocks it with DPAPI and prints the original password. |

Run 1 simulates the day you first configure the app. Run 2 simulates a normal day when the app starts and needs the password. Encrypting happens once; decrypting happens every start.

## Run it

You need Windows 10 or 11 and the [.NET SDK](https://dotnet.microsoft.com/download). This demo was run with .NET 10.

```
dotnet --version
cd DpapiConfigDemo
dotnet run
```

1. The first `dotnet run` prints a long jumbled text. Copy it.
2. Open `appsettings.json` and paste it between the quotes of `"EncryptedPassword"`.
3. Run `dotnet run` again. It now prints the decrypted password.

The project targets `net10.0`. If your SDK is a different version, change `TargetFramework` in the `.csproj` file to match, for example `net8.0`.

## Project files

| File | Purpose |
| --- | --- |
| `Program.cs` | Locks or unlocks the password depending on whether a locked value exists |
| `appsettings.json` | Sample config file that holds the locked password |
| `DpapiConfigDemo.csproj` | Project settings and the two NuGet packages used |

Packages: `System.Security.Cryptography.ProtectedData` for DPAPI and `Microsoft.Extensions.Configuration.Json` for reading the settings file.

## What DPAPI protects, and what it does not

- A copy of `appsettings.json` alone is useless to an attacker, because the locked text can only be decrypted by the same Windows user on the same machine.
- Any program running as the same Windows user can decrypt it. DPAPI protects the file at rest, not against malware running under your account.
- It is Windows only. For apps that may run on Linux or in containers, use the ASP.NET Core Data Protection API instead.
- The demo prints the decrypted password to the console. A real app would pass it to the database connection without printing it.

## Next steps

- Full-stack version: ASP.NET Core Web API reading from PostgreSQL with the database password protected by DPAPI, and a React frontend
- Cross-platform version using the ASP.NET Core Data Protection API
- Study of how attackers extract DPAPI-protected data, and how to defend against it

## Author

**Sumera Naaz Qureshi** — Cloud & DevOps Intern
GitHub: [sumeranaazq-29](https://github.com/sumeranaazq-29)
