using CQRS.Core.Commands;

namespace Post.Cmd.Infrastructure.Dispatchers;
public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Cannot register a handler, twice : " + typeof(T).FullName);
        }

        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (_handlers.TryGetValue(command.GetType(), out var handler))
        {
            await handler.Invoke(command);
        }

        throw new InvalidOperationException("Handler is not registered for: " + command.GetType().FullName);
    }
}
