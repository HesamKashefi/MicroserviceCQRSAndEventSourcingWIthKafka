using Confluent.Kafka;

using CQRS.Core.Consumers;

using Microsoft.EntityFrameworkCore;

using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repostirories;

namespace Post.Query.Api.Extensions.HostingExtensions;

public static class HostingExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentsRepository, CommentsRepostirory>();
        services.AddScoped<IEventHandler, Post.Query.Infrastructure.Handlers.EventHandler>();
        services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
        services.AddScoped<IEventConsumer, EventConsumer>();

        services.AddHostedService<ConsumerHosterService>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        Action<DbContextOptionsBuilder> configure = (options) => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
        services.AddDbContext<DatabaseContext>(configure);
        services.AddSingleton(new DatabaseContextFactory(configure));

        return services;
    }
}
