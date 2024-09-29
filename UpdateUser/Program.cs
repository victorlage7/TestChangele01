using Messaging;
using Messaging.Interface;
using UpdateUser;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

var host = builder.Build();
host.Run();
