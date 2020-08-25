<#
.SYNOPSIS
    Analyze a solution using the Docker analyzer image.
.DESCRIPTION
    Solutions on the website are analyzed using the Docker analyzer image.
    This script allows one to verify that this Docker image correctly
    analyzes a solution.
.PARAMETER Exercise
    The slug of the exercise to be analyzed.
.PARAMETER InputDirectory
    The directory in which the solution can be found.
.PARAMETER OutputDirectory
    The directory to which the analysis file will be written.
.EXAMPLE
    The example below will analyze the two-fer solution in the "~/exercism/two-fer" directory
    and write the results to "~/exercism/results/" using Docker
    PS C:\> ./analyze-in-docker.ps1 two-fer ~/exercism/two-fer ~/exercism/results/
#>

param (
    [Parameter(Position = 0, Mandatory = $true)]
    [string]$Exercise,

    [Parameter(Position = 1, Mandatory = $true)]
    [string]$InputDirectory,

    [Parameter(Position = 2, Mandatory = $true)]
    [string]$OutputDirectory
)

docker build -t exercism/csharp-analyzer .
docker run -v ${InputDirectory}:/input -v ${OutputDirectory}:/output exercism/csharp-analyzer $Exercise /input /output