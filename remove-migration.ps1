$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

$ContextProject = [System.IO.Path]::Combine($PSScriptRoot, "src/MaaCopilotServer.Infrastructure")
$StartupProject = [System.IO.Path]::Combine($PSScriptRoot, "src/MaaCopilotServer.Api")

dotnet ef migrations remove -p $ContextProject -s $StartupProject -f
