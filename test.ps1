<#
.SYNOPSIS
    Run all tests.
.DESCRIPTION
    Run all tests, verifying the behavior of the analyzer.
.PARAMETER UpdateAnalysis
    Update the expected analysis files to the current output (optional).
.PARAMETER UseDocker
    Run the tests using Docker (optional).
.EXAMPLE
    The example below will run all tests
    PS C:\> ./test.ps1

    The example below will run all tests using Docker
    PS C:\> ./test.ps1 -UseDocker

    The example below will run all tests and update both the analysis files
    PS C:\> ./test.ps1 -UpdateAnalysis
.NOTES
    The UpdateAnalysis switch should only be used if a bulk update of the 
    expected analysis files is needed.
#>

param (
    [Parameter(Mandatory = $false)]
    [Switch]$UpdateAnalysis,

    [Parameter(Mandatory = $false)]
    [Switch]$UseDocker
)

$Env:UPDATE_ANALYSIS = $UpdateAnalysis.IsPresent
$Env:USE_DOCKER = $UseDocker.IsPresent

if ($UseDocker.IsPresent) {
    docker build -t exercism/csharp-analyzer .
}

dotnet test

if ($UpdateAnalysis.IsPresent) {
    ./format.ps1
}

exit $LastExitCode