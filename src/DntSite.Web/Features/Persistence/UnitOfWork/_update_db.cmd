dotnet tool update --global dotnet-ef --version 10.0.9
dotnet tool restore
dotnet ef --verbose --project ../../../DntSite.Web.csproj --startup-project ../../../ database update
pause