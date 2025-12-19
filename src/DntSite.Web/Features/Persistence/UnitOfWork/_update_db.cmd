dotnet tool update --global dotnet-ef --version 10.0.1
dotnet tool restore
dotnet ef --verbose --project ../../../DntSite.Web.csproj --startup-project ../../../ database update
pause