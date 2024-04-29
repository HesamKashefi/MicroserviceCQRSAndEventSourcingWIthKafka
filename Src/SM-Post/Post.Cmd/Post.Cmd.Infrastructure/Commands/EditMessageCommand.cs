using CQRS.Core.Commands;

namespace Post.Cmd.Infrastructure.Commands
{
    public class EditMessageCommand : BaseCommand
    {
        public required string Message { get; set; }
    }
}