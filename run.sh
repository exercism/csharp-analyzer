#!/bin/sh
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
/opt/analyzer/Exercism.Analyzers.CSharp $1 $2 $3