dotnet restore --ignore-failed-sources --source %USERPROFILE%\.nuget\packages -v diag
dotnet tool restore --ignore-failed-sources --add-source %USERPROFILE%\.nuget\packages -v diag
pause