Remove-Item -Recurse -Force -Path ./publish

dotnet publish -c Release -o ./publish ./src/MaaCopilotServer.Api

Remove-Item -Recurse -Force -Path ./publish/*.pdb
Remove-Item -Recurse -Force -Path ./publish/*.Development.json
