using CQRS.Core.Messages;

namespace CQRS.Core.Events
{
    public abstract class BaseEvent(string type) : Message
    {
        public int Version { get; init; }
        public string Type { get; init; } = type;
    }
}