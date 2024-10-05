using CreateContact;
using Messaging.Interface;
using Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

var host = builder.Build();
host.Run();