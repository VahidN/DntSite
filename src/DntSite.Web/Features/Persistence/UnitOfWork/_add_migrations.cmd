dotnet tool update --global dotnet-ef --version 9.0.6
dotnet tool restore
For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%c_%%a_%%b)
For /f "tokens=1-2 delims=/:" %%a in ("%TIME: =0%") do (set mytime=%%a%%b)
dotnet ef --verbose --project ../../../DntSite.Web.csproj --startup-project ../../../ migrations add V%mydate%_%mytime% --output-dir Features/Persistence/Migrations
pause