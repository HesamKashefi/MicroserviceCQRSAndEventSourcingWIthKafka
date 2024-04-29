
using CQRS.Core.Events;

namespace Post.Query.Infrastructure.Events
{
    public class MessageUpdatedEvent : BaseEvent
    {
        public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent))
        {
        }

        public required string Message { get; init; }
    }
}