Remove-Item Data/game.db
Remove-Item Data/Migrations/*.cs
dotnet ef migrations add init -o Data\Migrations\
dotnet ef database update
