using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler(IEventStore eventStore) : IEventSourcingHandler<PostAggregate>
    {
        private readonly IEventStore _eventStore = eventStore;
        public async Task<PostAggregate> GetByIdAsync(Guid id)
        {
            PostAggregate postAggregate = new();

            var events = await _eventStore.GetEventsAsync(id);

            if (events is null || events.Count == 0)
            {
                return postAggregate;
            }

            postAggregate.ReplayEvents(events);
            postAggregate.Version = events.Select(x => x.Version).Max();

            return postAggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUnCommitedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommited();
        }
    }
}