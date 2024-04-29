using CQRS.Core.Events;

namespace Post.Common.Events;
public class CommentUpdatedEvent : BaseEvent
{
    public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
    {
    }
    public Guid CommentId { get; init; }
    public required string Comment { get; init; }
    public required string Username { get; init; }
    public DateTime EditDate { get; init; }
}
