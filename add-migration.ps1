[CmdletBinding()]
Param(
  [Parameter(Position = 0, Mandatory = $false)]
  [string]
  $MigrationName
)

$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

$ContextProject = [System.IO.Path]::Combine($PSScriptRoot, "src/MaaCopilotServer.Infrastructure")
$StartupProject= [System.IO.Path]::Combine($PSScriptRoot, "src/MaaCopilotServer.Api")

dotnet ef migrations add $MigrationName -p $ContextProject -s $StartupProject
