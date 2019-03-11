#!/bin/bash

# Usage:
# ./bin/analyze.sh two_fer ~/test/

dotnet run -c Release -p ./src/Exercism.Analyzers.CSharp $1 $2