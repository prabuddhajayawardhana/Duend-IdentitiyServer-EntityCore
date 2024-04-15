Duende IdentityServer: The most flexible and standards-compliant OpenID Connect and OAuth 2.0 framework for ASP.NET Core and React with EnityFremworkCore 

    dotnet ef migrations add InitialCreate -c ConfigurationDbContext
    dotnet ef migrations add InitialCreate -c PersistedGrantDbContext
    dotnet ef migrations add InitialCreate -c ApplicationDbContext
    
    dotnet ef database update -c PersistedGrantDbContext
    dotnet ef database update -c ConfigurationDbContext
    dotnet ef database update -c ApplicationDbContext
    
    dotnet run bin/Debug/net6.0/ids /seed`
