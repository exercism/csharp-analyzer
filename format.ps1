<#
.SYNOPSIS
    Format the source code.
.DESCRIPTION
    Formats the .NET source code, as well as all markdown and JSON files.
.EXAMPLE
    The example below will format all source code
    PS C:\> ./format.ps1
.NOTES
    The formatting of markdown and JSON files is done through prettier. This means
    that NPM has to be installed for this functionality to work.
#>

dotnet tool install -g dotnet-format --version 3.0.4
dotnet format

npx prettier@1.18.2 --write "**/*.{json,md}"