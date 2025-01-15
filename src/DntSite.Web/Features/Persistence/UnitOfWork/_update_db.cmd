dotnet tool update --global dotnet-ef --version 9.0.1
dotnet tool restore
dotnet ef --verbose --project ../../../DntSite.Web.csproj --startup-project ../../../ database update
pause