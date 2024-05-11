using Confluent.Kafka;

using Post.Cmd.Api.Extensions.HostingExtensions;
using Post.Cmd.Infrastructure.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection(nameof(MongoConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();