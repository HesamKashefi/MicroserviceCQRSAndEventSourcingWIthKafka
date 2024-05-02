using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<EventModel> _collection;
        public EventStoreRepository(IOptions<MongoConfig> options)
        {
            var client = new MongoClient(options.Value.ConnnectionString);
            var db = client.GetDatabase(options.Value.Database);
            _collection = db.GetCollection<EventModel>(options.Value.Collection);
        }

        public async Task<List<EventModel>> FindByAggregateIdAsync(Guid aggregateId)
        {
            return await _collection
            .Find(x => x.AggregateIdentified == aggregateId)
            .ToListAsync()
            .ConfigureAwait(false);
        }

        public async Task SaveAsync(EventModel @event)
        {
            await _collection.InsertOneAsync(@event).ConfigureAwait(false);
        }
    }
}