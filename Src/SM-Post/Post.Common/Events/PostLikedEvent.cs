
using CQRS.Core.Events;

namespace Post.Query.Infrastructure.Events
{
    public class PostLikedEvent : BaseEvent
    {
        public PostLikedEvent() : base(nameof(PostLikedEvent))
        {
        }
    }
}