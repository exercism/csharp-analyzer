<#
.SYNOPSIS
    Run all tests.

.DESCRIPTION
    Run all tests, verifying the behavior of the analyzer.

.PARAMETER UpdateAnalysis
    Update the expected analysis files to the current output.

.PARAMETER UpdateComments
    Update the comment files to the current output.

.EXAMPLE
    The example below will run all tests
    PS C:\> ./test.ps1

    The example below will run all tests and update the comments
    PS C:\> ./test.ps1 -UpdateComments

    The example below will run all tests and update both the comments and analysis files
    PS C:\> ./test.ps1 -UpdateComments -UpdateAnalysis
#>

param (
    [Parameter(Mandatory = $false)]
    [Switch]$UpdateAnalysis,

    [Parameter(Mandatory = $false)]
    [Switch]$UpdateComments
)

git submodule update --remote --merge website-copy

$Env:UPDATE_ANALYSIS = $UpdateAnalysis.IsPresent
$Env:UPDATE_COMMENTS = $UpdateComments.IsPresent

dotnet test