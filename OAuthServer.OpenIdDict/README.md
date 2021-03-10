# openid dict demo

This is an ASP.NET Core OAuth authorization_flow demo using [OpenidDict][1], Postgres, EntityFramework, ASP.NET Identity.

This flow is the most common flow, where you call `/connect/authorize`, you login via your browser first, it sets a cookie and then you get a code back which you use with `/connect/token` - a call which is typically done via the back channel, in other words server-side as you are passing in a client secret.

## Development testing

### Postgres

- Install Docker for Windows/Mac
- docker run -d -p 8080:8080 adminer
- docker run -d -p 5432:5432 --name openid-postgres -e POSTGRES_USER=openiddict -e POSTGRES_PASSWORD=openiddict -e POSTGRES_DB=openiddict postgres
- Run the project
- To see the postgres tables that have been created, go to http://localhost:8080 and use the above details. **For the db field, use your IP address (not 127.0.0.1)**

### Testing
- `cd src`
- `dotnet publish -c Release`
- `docker-compose up`
- Go to https://oidcdebugger.com/ 
- Authorize Uri: https://localhost:5000/connect/authorize
- ClientId: "client1"
- Scope: "openid"
- Response type: "code"
- Response mode: "form_post"
- Click "Send Request"
- Login with "test@localhost" / Password123=
- Open tokens.http in VS Code.
- Replace the code you got from oidebugger.com
- Run it, see the JWTs returned.

## Nuget packages and tools

- OpenId Dict
  - Currently using `3.0.0-beta6.20527.75`
  - `OpenIddict.AspNetCore`
  - `OpenIddict.EntityFrameworkCore`
- Postgres
  - `Npgsql.EntityFrameworkCore.PostgreSQL`
- ASP.NET Identity (for storing and retrieving users, logging them in)
  - `Microsoft.AspNetCore.Identity`
  - `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- EntityFrameworkCore
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.Abstractions`
- EntityFramework migrations
  - `Microsoft.EntityFrameworkCore.Design` (required for migrations)
  - `dotnet tool install --global dotnet-ef --version 5.0.1`
  - Delete the Data/Migrations folder
  - `dotnet ef migrations add InititalPersistedGrantDbMigration -c ApplicationDbContext -o Data/Migrations`
  - `dotnet ef database update`

[1]: https://github.com/openiddict/openiddict-core