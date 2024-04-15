using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDataProtection()
//    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\keyDirectory"));
builder.Services.AddAuthentication();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthentication(
  options =>
  {
      options.DefaultScheme = "cookie";
      options.DefaultChallengeScheme = "oidc";
  }).AddCookie("cookie")
  .AddOpenIdConnect("oidc", options =>
  {
      options.Authority = "https://localhost:5001";
      options.ClientId = "interactive";
      options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
      options.Scope.Add("weatherapi.read");

      options.ResponseType = "code";
      options.UsePkce = true;
      options.ResponseMode = "query";
      options.SaveTokens = true;

  });

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowAllOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
