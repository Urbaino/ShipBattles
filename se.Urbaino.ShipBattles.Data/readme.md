
dotnet ef migrations add [name]

dotnet ef migrations remove

dotnet ef database update 

--startup-project ..\se.Urbaino.ShipBattles.Web\se.Urbaino.ShipBattles.Web.csproj



dotnet ef migrations add [name] --startup-project ..\se.Urbaino.ShipBattles.Web\se.Urbaino.ShipBattles.Web.csproj

dotnet ef migrations remove --startup-project ..\se.Urbaino.ShipBattles.Web\se.Urbaino.ShipBattles.Web.csproj

dotnet ef database update --startup-project ..\se.Urbaino.ShipBattles.Web\se.Urbaino.ShipBattles.Web.csproj
