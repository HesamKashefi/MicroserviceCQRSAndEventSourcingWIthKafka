
using CQRS.Core.Events;

namespace Post.Query.Infrastructure.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        public CommentAddedEvent() : base(nameof(CommentAddedEvent))
        {
        }
        public Guid CommentId { get; init; }
        public required string Comment { get; init; }
        public required string Username { get; init; }
        public DateTime CommentDate { get; init; }
    }
}