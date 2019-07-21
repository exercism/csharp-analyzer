# Exercism C# analyzer

A tool that can analyze C# solutions submitted to [Exercism](https://exercism.io).

## Analyzing a solution

To analyze a solution, follow these steps:

1. Open a command prompt in the root directory.
1. Run `./analyze.ps1 <slug> <directory>`. This script will run the analyzer on the specified directory.
1. Once the script has completed, the analysis results will be written to `<directory>/analysis.json`.

## Analyzing multiple solutions

To analyze multiple solutions at once, follow these steps:

1. Open a command prompt in the root directory.
1. Run `./bulk-analyze.ps1 <slug> <directory>`. This script will run the analyzer on each directory sub-directory of `<directory>`.
1. Once the script has completed, it will:
   1. Output general staticics to the console.
   1. Write detailed analysis results to `<directory>/bulk_analysis.json`.

## Analyzing a solution using Docker

To analyze a solution using a Docker container, follow these steps:

1. Open a command prompt in the root directory.
1. Run `./run-in-docker.ps1 <slug> <directory>`. This script will:
   1. Build the analyzer Docker image (if necessary).
   1. Run the analyzer Docker image (as a container), passing the specified `slug` and `directory` arguments.
1. Once the script has completed, the analysis result can be found at `<directory>/analysis.json`.

Note that the Docker image is built using the [.NET IL Linker](https://github.com/dotnet/core/blob/master/samples/linker-instructions.md#using-the-net-il-linker), which is why building can be quite slow.

## Source code formatting

This repository uses the [dotnet-format tool](https://github.com/dotnet/format/) to format the source code. There are no custom rules; we just use the default formatting. You can format the code by running the `./format.ps1` command.

### Scripts

The scripts in this repository are written in PowerShell. As PowerShell is cross-platform nowadays, you can also install it on [Linux](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux?view=powershell-6) and [macOS](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-macos?view=powershell-6).
