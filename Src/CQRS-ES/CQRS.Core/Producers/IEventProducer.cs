using CQRS.Core.Events;

namespace CQRS.Core.Producers;
public interface IEventProducer
{
    Task Produce<T>(string topic, T @event) where T : BaseEvent;
}
