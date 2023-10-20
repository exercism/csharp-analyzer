#!/usr/bin/env sh

# Synopsis:
# Update the golden tests.

# Example:
# ./bin/update-golden-tests.sh

# Generate the up-to-date analysis.json and tags.json files
./bin/run-tests-in-docker.sh

# Overwrite the existing files
find tests -name analysis.json -execdir cp analysis.json expected_analysis.json \;
find tests -name tags.json -execdir cp tags.json expected_tags.json \;
