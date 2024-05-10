using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers
{
    public class EventConsumer(IEventHandler eventHandler, IOptions<ConsumerConfig> options) : IEventConsumer
    {
        private readonly IEventHandler _eventHandler = eventHandler;
        private readonly ConsumerConfig _config = options.Value;

        public void Consume(string topic)
        {
            var jsonOptions = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };

            var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

            consumer.Subscribe(topic);
            while (true)
            {
                var consumeResult = consumer.Consume();

                if (consumeResult?.Message is null) continue;

                var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, jsonOptions);

                var method = _eventHandler.GetType().GetMethod("On", [@event!.GetType()]) ??
                    throw new InvalidOperationException($"Could not fine the 'On' on {nameof(IEventHandler)} method for event : '{@event!.GetType().Name}'");

                method.Invoke(_eventHandler, [@event]);

                consumer.Commit(consumeResult);
            }
        }
    }
}