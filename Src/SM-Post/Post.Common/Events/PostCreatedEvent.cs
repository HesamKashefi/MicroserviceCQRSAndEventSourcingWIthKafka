
using CQRS.Core.Events;

namespace Post.Query.Infrastructure.Events
{
    public class PostCreatedEvent : BaseEvent
    {
        public PostCreatedEvent() : base(nameof(PostCreatedEvent))
        {
        }

        public required string Author { get; init; }
        public required string Message { get; init; }
        public DateTime DatePosted { get; init; }
    }
}