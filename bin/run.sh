#!/usr/bin/env sh

# Synopsis:
# Run the analyzer on a solution.

# Arguments:
# $1: exercise slug
# $2: path to solution folder
# $3: path to output directory

# Output:
# Writes the analysis results to a analysis.json file in the passed-in output directory.
# The analysis results are formatted according to the specifications at https://github.com/exercism/docs/blob/main/building/tooling/analyzers/interface.md

# Example:
# ./bin/run.sh two-fer path/to/solution/folder/ path/to/output/directory/

# If any required arguments is missing, print the usage and exit
if [ "$#" -lt 3 ]; then
    echo "usage: ./bin/run.sh exercise-slug path/to/solution/folder/ path/to/output/directory/"
    exit 1
fi

export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

if [ -f /opt/analyzer/Exercism.Analyzers.CSharp ]; then
    /opt/analyzer/Exercism.Analyzers.CSharp $1 $2 $3
else
    dotnet run --project ./src/Exercism.Analyzers.CSharp/ $1 $2 $3
fi
