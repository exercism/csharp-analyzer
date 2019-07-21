<#
.SYNOPSIS
    Analyze a solution.

.DESCRIPTION
    Analyze a solution.

.PARAMETER Slug
    The slug of the exercise to be analyzed.

.PARAMETER Directory
    The directory in which the solution can be found.

.EXAMPLE
    The example below will analyze the two-fer solution in the "~/exercism/two-fer" directory
    PS C:\> ./analyze.ps1 -Slug two-fer -Directory ~/exercism/two-fer
#>

param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$Slug, 
    
    [Parameter(Position = 1, Mandatory = $true)]
    [string]$Directory
)

dotnet run --project ./src/Exercism.Analyzers.CSharp/ $Slug $Directory