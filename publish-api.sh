rm -rf ./publish

dotnet publish -c Release -o ./publish ./src/MaaCopilotServer.Api

rm -rf ./publish/*.pdb
rm -rf ./publish/*.Development.json
