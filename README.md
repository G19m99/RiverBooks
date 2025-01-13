RiverBooks following along course on dometrain

use full cli commands:

- create a new migrations
  dotnet ef migrations add InitialUsers -c UsersDbContext -p ..\RiverBooks.Users\RiverBooks.Users.csproj -s .\RiverBooks.Web.csproj -o Data/Migrations

  this will create a users module migartion

- Update the database with new migration
  dotnet ef database update -c UsersDbContext

  
