$Slug = $Args[0]
$Directory = $Args[1]

docker build -t exercism/csharp-analyzer .
docker run -v ${Directory}:/solution exercism/csharp-analyzer ${Slug} /solution