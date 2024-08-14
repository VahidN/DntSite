dotnet tool update --global dotnet-ef --version 8.0.8
dotnet tool restore
dotnet ef --verbose --project ../../../DntSite.Web.csproj --startup-project ../../../ database update
pause