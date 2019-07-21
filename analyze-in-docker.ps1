<#
.SYNOPSIS
    Analyze a solution using the Docker analyzer image.

.DESCRIPTION
    Solutions on the website are analyzed using the Docker analyzer image.
    This script allows one to verify that this Docker image correctly
    analyzes a solution.

.PARAMETER Slug
    The slug of the exercise to be analyzed.

.PARAMETER Directory
    The directory in which the solution can be found.

.EXAMPLE
    The example below will analyze the two-fer solution in the "~/exercism/two-fer" directory
    PS C:\> ./analyze-in-docker.ps1 -Slug two-fer -Directory ~/exercism/two-fer
#>

param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$Slug, 
    
    [Parameter(Position = 1, Mandatory = $true)]
    [string]$Directory
)

docker build -t exercism/csharp-analyzer .
docker run -v ${Directory}:/solution exercism/csharp-analyzer ${Slug} /solution