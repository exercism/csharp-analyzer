# Exercism C# analyzer
A tool that can analyze C# exercise solutions.

## Build and running in Docker

1. Open a command prompt in `src/Exercism.Analyzers.CSharp`
1. Build the Docker image using: `docker build -t exercism/csharp-analyzer .`
1. Run the Docker image using: `docker run -v <directory>:/<slug> exercism/csharp-analyzer <slug> /<slug>`. An example of this command is: `docker run -v ~/exercism/csharp/two-fer:/two-fer exercism/csharp-analyzer two-fer /two-fer`.

The C# analyzer's Docker image will then run and analyze the solution.

## Source code formatting

This repository uses the [dotnet-format tool](https://github.com/dotnet/format/) to format the source code. There are no custom rules; we just use the default formatting. You can format the code by running the `format.ps1` command.

### Scripts

The scripts in this repository are written in PowerShell. As PowerShell is cross-platform nowadays, you can also install it on [Linux](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux?view=powershell-6) and [macOS](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-macos?view=powershell-6).