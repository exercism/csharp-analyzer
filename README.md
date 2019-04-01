# Exercism C# analyzer
A tool that can analyze C# exercise solutions.

## Build and running in Docker

1. Open a command prompt in the root directory.
1. Run `./run-in-docker.ps1 <slug> <directory>`. This script will:
   1. Build the analyzer Docker image (if necessary).
   1. Run the analyzer Docker image (as a container), passing the specified `slug` and `directory` arguments.
1. Once the script has completed, the analysis result can be found at `<directory>/analysis.json`.

## Source code formatting

This repository uses the [dotnet-format tool](https://github.com/dotnet/format/) to format the source code. There are no custom rules; we just use the default formatting. You can format the code by running the `./format.ps1` command.

### Scripts

The scripts in this repository are written in PowerShell. As PowerShell is cross-platform nowadays, you can also install it on [Linux](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux?view=powershell-6) and [macOS](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-macos?view=powershell-6).