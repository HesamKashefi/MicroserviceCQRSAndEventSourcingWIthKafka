using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore(IEventStoreRepository eventStoreRepository) : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository = eventStoreRepository;

        public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
        {
            var aggregate = await _eventStoreRepository.FindByAggregateIdAsync(aggregateId);

            if (aggregate is null || !aggregate.Any())
            {
                throw new AggregateNotFoundException("Incorrect Post ID provided");
            }

            return aggregate.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var aggregate = await _eventStoreRepository.FindByAggregateIdAsync(aggregateId);

            if (expectedVersion != -1 && aggregate[^1].Version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            var version = expectedVersion;

            foreach (var @event in events)
            {
                var model = new EventModel
                {
                    AggregateIdentified = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    EventType = @event.GetType().Name,
                    EventData = @event,
                    Version = ++version,
                    TimeStamp = DateTime.Now
                };

                await _eventStoreRepository.SaveAsync(model);
            }
        }
    }
}