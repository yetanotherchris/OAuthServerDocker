# OAuthServerDocker
A pre-configured OAuth Server that runs in Docker with, using .NET Core IdentityServer in memory test quickstart.

This server isn't intended for production use, just testing OAuth/OpenId Connect flows. The source is based off running the dotnet tool:

```
dotnet new -i IdentityServer4.Templates
dotnet new is4inmem
```

## Quickstart

docker run todo

## Debugging

docker compose details todo

## Configuration

- If you want to add or modify clients, you can do this in the clients.json file.
- If you want to add or modify users, you can do this in the users.json file.
- If you want to add or modify API scopes (the client_credentials grant type aka service flow) in the apis.json file.