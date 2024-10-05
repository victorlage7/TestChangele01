using ContactServiceAPi;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);


var app = builder.Build();

startup.Configure(app, app.Environment);

// Adicione o Prometheus middleware
app.UseHttpMetrics();
app.UseMetricServer(); // Exponha métricas na rota /metrics

app.MapControllers();

app.Run();