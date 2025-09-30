# RiverBooks

A test project experimenting with modular monoliths (Based on ardalis)

## Run Locally

Run redis

```bash
  docker run --name my-redis -p 6379:6379 -d redis
```

Go to the RiverBooks.Web directory

```bash
  cd src/RiverBooks.Web
```

Run migrations to update DB

```bash
  dotnet ef database update -c UsersDbContext
  dotnet ef database update -c OrderProcessingDbContext
  dotnet ef database update -c BookDbContext

```

Start the server

```bash
  dotnet run
```

Test endpoints using the http file in RiverBooks.Web project
