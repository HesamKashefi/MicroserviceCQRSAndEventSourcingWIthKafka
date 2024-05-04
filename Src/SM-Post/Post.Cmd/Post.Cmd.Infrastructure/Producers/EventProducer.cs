using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Producers;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Post.Cmd.Infrastructure.Producers;
public class EventProducer(IOptions<ProducerConfig> options) : IEventProducer
{
    private readonly ProducerConfig _config = options.Value;

    public async Task Produce<T>(string topic, T @event) where T : BaseEvent
    {
        var producer = new ProducerBuilder<string, string>(_config)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();

        var message = new Message<string, string>()
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType())
        };

        var deliveryResult = await producer.ProduceAsync(topic, message);

        if (deliveryResult.Status != PersistenceStatus.NotPersisted)
        {
            throw new Exception($"Could not produce event: '{@event.GetType().Name}' to topic: '{topic}' due to reason: '{deliveryResult.Message}'");
        }
    }
}
