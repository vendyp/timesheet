dotnet restore TimesheetAll.sln

dotnet build TimesheetAll.sln --no-restore /warnaserror

dotnet test TimesheetAll.sln