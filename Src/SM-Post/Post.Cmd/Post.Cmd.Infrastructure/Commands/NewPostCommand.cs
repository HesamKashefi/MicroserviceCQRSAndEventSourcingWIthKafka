using CQRS.Core.Commands;

namespace Post.Cmd.Infrastructure.Commands
{
    public class NewPostCommand : BaseCommand
    {
        public required string Author { get; set; }
        public required string Message { get; set; }
    }
}