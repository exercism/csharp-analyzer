# Exercism C# analyzer
A tool that can analyze C# exercise solutions.

## Build and running in Docker

1. Open a command prompt in `src/Exercism.Analyzers.CSharp`
1. Build the Docker image using: `docker build -t exercism/csharp-analyzer .`
1. Run the Docker image using: `docker run -v <directory>:/<slug> exercism/csharp-analyzer <slug> /<slug>`. An example of this command is: `docker run -v ~/exercism/csharp/two-fer:/two-fer exercism/csharp-analyzer two-fer /two-fer`.

The C# analyzer's Docker image will then run and analyze the solution.