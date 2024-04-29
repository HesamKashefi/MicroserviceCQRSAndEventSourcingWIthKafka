using CQRS.Core.Commands;

namespace Post.Cmd.Infrastructure.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        public required string Username { get; set; }
    }
}