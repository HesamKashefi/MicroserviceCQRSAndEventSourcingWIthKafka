using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events
{
    public class EventModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        public DateTime TimeStamp { get; init; }
        public int Version { get; init; }
        public required Guid AggregateIdentifier { get; init; }
        public required string AggregateType { get; init; }
        public required string EventType { get; init; }
        public required BaseEvent EventData { get; init; }
    }
}