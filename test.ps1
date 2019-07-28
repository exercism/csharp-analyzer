<#
.SYNOPSIS
    Run all tests.
.DESCRIPTION
    Run all tests, verifying the behavior of the analyzer.
.PARAMETER UpdateAnalysis
    Update the expected analysis files to the current output (optional).
.PARAMETER UpdateComments
    Update the comment files to the current output (optional).
.EXAMPLE
    The example below will run all tests
    PS C:\> ./test.ps1

    The example below will run all tests and update the comments
    PS C:\> ./test.ps1 -UpdateComments

    The example below will run all tests and update both the comments and analysis files
    PS C:\> ./test.ps1 -UpdateComments -UpdateAnalysis
.NOTES
    The UpdateAnalysis and UpdateComments switches should only be used if
    a bulk update of the expected analysis and/or expected comments files is needed.
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

if ($UpdateAnalysis.IsPresent -or $UpdateComments.IsPresent) {
    ./format.ps1
}