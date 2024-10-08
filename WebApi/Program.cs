using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using WebApi;
using WebApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var securityKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretJwt"));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(securityKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

//Aplica migra��es a
await app.MigrateDatabaseAsync();


// Adicione o Prometheus middleware
app.UseHttpMetrics();
app.UseMetricServer(); // Exponha métricas na rota /metrics

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}