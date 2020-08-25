<#
.SYNOPSIS
    Analyze a solution.
.DESCRIPTION
    Analyze a solution.
.PARAMETER Slug
    The slug of the exercise to be analyzed.
.PARAMETER InputDirectory
    The directory in which the solution can be found.
.PARAMETER OutputDirectory
    The directory to which the analysis file will be written.
.EXAMPLE
    The example below will analyze the two-fer solution in the "~/exercism/two-fer" directory
    and write the results to "~/exercism/results/" using Docker
    PS C:\> ./analyze.ps1 two-fer ~/exercism/two-fer ~/exercism/results/
#>

param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$Exercise,

    [Parameter(Position = 1, Mandatory = $true)]
    [string]$InputDirectory,

    [Parameter(Position = 2, Mandatory = $true)]
    [string]$OutputDirectory
)

dotnet run --project ./src/Exercism.Analyzers.CSharp/ $Exercise $InputDirectory $OutputDirectory