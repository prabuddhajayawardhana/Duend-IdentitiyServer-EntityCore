using IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//builder.Services.AddDataProtection()
//    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\keyDirectory"));

var connectionString = builder.Configuration.GetConnectionString("defaultConnection");

var migrationsAssembly = typeof(Config).Assembly.GetName().Name;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;

        options.EmitStaticAudienceClaim = true;
    }).AddConfigurationStore(option => option.ConfigureDbContext = b => b.UseSqlite(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly)))
    .AddOperationalStore(option => option.ConfigureDbContext = b => b.UseSqlite(connectionString, opt => opt.MigrationsAssembly(migrationsAssembly)))
      .AddAspNetIdentity<IdentityUser>();


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication();
builder.Services.AddRazorPages();


builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Debug()
      .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
      .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
      .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
      .MinimumLevel.Override("System", LogEventLevel.Warning)
      .WriteTo.Console(
        outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
        theme: AnsiConsoleTheme.Code)
      .Enrich.FromLogContext();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseIdentityServer();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (args.Contains("/seed"))
{
    Log.Information("Seeding database...");
    SeedData.EnsureSeedData(app);
    Log.Information("Done seeding database. Exiting.");
    return;
}

app.Run();
