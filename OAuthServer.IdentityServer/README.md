# OAuthServerDocker
A pre-configured OAuth Server that runs in Docker with, using .NET Core IdentityServer in memory test quickstart.

This server isn't intended for production use, just testing OAuth/OpenId Connect flows. The source is based off running the dotnet tool:

```
dotnet new -i IdentityServer4.Templates
dotnet new is4inmem
```

### Testing
- `cd src`
- `dotnet run`
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

## Configuration

- If you want to add or modify clients, you can do this in the clients.json file.
- If you want to add or modify users, you can do this in the users.json file.
- If you want to add or modify API scopes (the client_credentials grant type aka service flow) in the apis.json file.