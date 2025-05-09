@echo off
echo Creating a new migration...
set /p name="Enter migration name: "
dotnet ef migrations add %name% --project ECommerceApp.Infrastructure --startup-project ECommerceApp.Web
echo.
echo Applying migration to database...
dotnet ef database update --project ECommerceApp.Infrastructure --startup-project ECommerceApp.Web
echo.
echo Migration complete.
pause 