## DB commands
All these commands are to be run in the se.Urbaino.ShipBattles.Data directory.

# Create a new migration
dotnet ef migrations add [name] --startup-project ..\se.Urbaino.ShipBattles.Web\se.Urbaino.ShipBattles.Web.csproj

# Delete an existing migration
dotnet ef migrations remove --startup-project ..\se.Urbaino.ShipBattles.Web\se.Urbaino.ShipBattles.Web.csproj

# Apply migrations (also to create the database)
dotnet ef database update --startup-project ..\se.Urbaino.ShipBattles.Web\se.Urbaino.ShipBattles.Web.csproj
