using DeleteUser;
using Messaging;
using Messaging.Interface;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

var host = builder.Build();
host.Run();
