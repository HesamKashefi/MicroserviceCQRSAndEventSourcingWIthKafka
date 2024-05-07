using CQRS.Core.Commands;

namespace Post.Cmd.Infrastructure.Dispatchers;
public interface ICommandDispatcher
{
    void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;

    Task SendAsync(BaseCommand command);
}
