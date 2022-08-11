# Use Development environment for testing locally.
# The appsettings.json will become appsettings.Development.json.
$Environment = "Development"

$ApiProjectPath = ".\src\MaaCopilotServer.Api"
$AppSettingsDev = "$ApiProjectPath\appsettings.$Environment.json"
$ApiProject = "$ApiProjectPath\MaaCopilotServer.Api.csproj"

$PublishPath = ".\publish"

$BuildConfiguration = "Debug"
$DockerBuildName = "maa-copilot-server:dev"

# Make sure you have the appsettings.Development.json file.

# If you want to connect to the host form docker, remember to replace
# the localhost or 127.0.0.1 in the JSON with host.docker.internal.
If (-Not (Test-Path -Path "$AppSettingsDev")) {
    Write-Error "The file ""$AppSettingsDev"" does not exist."
    Write-Host "Make sure you have created the file for testing."
    Write-Host "If you want to connect to the host form docker, remember to replace the localhost or 127.0.0.1 in the JSON with host.docker.internal."
    Exit 1
}

dotnet build -c "$BuildConfiguration" "$ApiProject"
Remove-Item -Recurse -Force -Path "$PublishPath"
dotnet publish -c "$BuildConfiguration" -o "$PublishPath" "$ApiProject"
docker build -t "$DockerBuildName" .
docker run --rm -p 80:80 --env ASPNETCORE_ENVIRONMENT=$Environment "$DockerBuildName"
