using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers;

public class EventHandler(IPostRepository postRepository, ICommentsRepository commentsRepository) : IEventHandler
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ICommentsRepository _commentsRepository = commentsRepository;

    public async Task On(PostCreatedEvent @event)
    {
        PostEntity post = new()
        {
            PostId = @event.Id,
            Author = @event.Author,
            Message = @event.Message,
            DatePosted = @event.DatePosted,
            Likes = 0,
            Comments = []
        };
        await _postRepository.CreateAsync(post);
    }

    public async Task On(MessageUpdatedEvent @event)
    {
        PostEntity? post = await _postRepository.GetByIdAsync(@event.Id);

        if (post is null) return;

        post.Message = @event.Message;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(PostLikedEvent @event)
    {
        PostEntity? post = await _postRepository.GetByIdAsync(@event.Id);

        if (post is null) return;

        post.Likes++;
        await _postRepository.UpdateAsync(post);
    }

    public async Task On(CommentAddedEvent @event)
    {
        CommentEntity comment = new()
        {
            PostId = @event.Id,
            Username = @event.Username,
            Comment = @event.Comment,
            CommentDate = @event.CommentDate,
            CommentId = @event.CommentId,
        };
        await _commentsRepository.CreateAsync(comment);
    }

    public async Task On(CommentUpdatedEvent @event)
    {
        CommentEntity? comment = await _commentsRepository.GetByIdAsync(@event.Id);

        if (comment is null) return;

        comment.Comment = @event.Comment;
        comment.CommentDate = @event.EditDate;
        comment.Edited = true;
        await _commentsRepository.UpdateAsync(comment);
    }

    public async Task On(CommentRemovedEvent @event)
    {
        CommentEntity? comment = await _commentsRepository.GetByIdAsync(@event.Id);

        if (comment is null) return;

        await _commentsRepository.DeleteAsync(comment);
    }

    public async Task On(PostRemovedEvent @event)
    {
        PostEntity? post = await _postRepository.GetByIdAsync(@event.Id);

        if (post is null) return;

        await _postRepository.DeleteAsync(post);
    }
}
