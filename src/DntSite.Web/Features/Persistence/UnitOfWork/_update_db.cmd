dotnet tool update --global dotnet-ef --version 9.0.7
dotnet tool restore
dotnet ef --verbose --project ../../../DntSite.Web.csproj --startup-project ../../../ database update
pause