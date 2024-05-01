using CQRS.Core.Events;

namespace CQRS.Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        public Guid Id => _id;

        public int Version { get; set; } = -1;

        private readonly List<BaseEvent> _changes = [];

        public IReadOnlyList<BaseEvent> GetUnCommitedChanges() => _changes.AsReadOnly();

        public void MarkChangesAsCommited() => _changes.Clear();

        private void Apply(BaseEvent @event, bool isNew)
        {
            var method = GetType().GetMethod("Apply", [@event.GetType()]);

            if (method is null)
            {
                throw new InvalidOperationException("The apply method was not found for " + @event.GetType().Name);
            }

            method.Invoke(this, [@event]);

            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        public void RaiseEvent(BaseEvent @event)
        {
            Apply(@event, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var @event in events)
            {
                Apply(@event, false);
            }
        }
    }
}