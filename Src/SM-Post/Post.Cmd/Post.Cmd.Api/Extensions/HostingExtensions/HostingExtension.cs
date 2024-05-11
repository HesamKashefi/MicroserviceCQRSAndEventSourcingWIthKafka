using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;

using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Producers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

namespace Post.Cmd.Api.Extensions.HostingExtensions;

public static class HostingExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddScoped<IEventStoreRepository, EventStoreRepository>();
        services.AddScoped<IEventStore, EventStore>();
        services.AddScoped<IEventProducer, EventProducer>();
        services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
        services.AddScoped<ICommandHandler, CommandHandler>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // register dispatcher handlers
        services.AddSingleton<ICommandDispatcher>(sp =>
        {
            var scope = sp.CreateScope();
            var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler>();

            var dispatcher = new CommandDispatcher();

            dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
            dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);

            return dispatcher;
        });

        return services;
    }
}
