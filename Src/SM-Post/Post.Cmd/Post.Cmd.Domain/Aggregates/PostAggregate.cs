using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;
        private string _author = "";

        private readonly Dictionary<Guid, Tuple<string, string>> _comments = [];

        public bool Active { get => _active; set => _active = value; }

        public PostAggregate()
        {

        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent { Id = id, Author = author, Message = message, DatePosted = DateTime.Now });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _active = true;
            _id = @event.Id;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if (_active == false)
            {
                throw new InvalidOperationException("You cannot change the message of an inactive post");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent()
            {
                Id = _id,
                Message = message
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void LikePost()
        {
            if (_active == false)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }

            RaiseEvent(new PostLikedEvent { Id = _id });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (_active == false)
            {
                throw new InvalidOperationException("You cannot add comment to an inactive post");
            }

            if (string.IsNullOrEmpty(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
            }

            RaiseEvent(new CommentAddedEvent()
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (_active == false)
            {
                throw new InvalidOperationException("You cannot edit a comment of an inactive post");
            }

            if (!_comments.ContainsKey(commentId))
            {
                throw new InvalidOperationException($"Comment with id={commentId} was not found on post {_id}");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException($"Comment with id={commentId} on post {_id} does not belong to {username}");
            }

            RaiseEvent(new CommentUpdatedEvent { Id = _id, CommentId = commentId, Username = username, Comment = comment, EditDate = DateTime.Now });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void DeleteComment(Guid commentId, string username)
        {
            if (_active == false)
            {
                throw new InvalidOperationException("You cannot remove a comment of an inactive post");
            }

            if (!_comments.ContainsKey(commentId))
            {
                throw new InvalidOperationException($"Comment with id={commentId} was not found on post {_id}");
            }


            if (!_comments[commentId].Item2.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException($"Comment with id={commentId} on post {_id} does not belong to {username}");
            }

            RaiseEvent(new CommentRemovedEvent { Id = _id, CommentId = commentId });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if (_active == false)
            {
                throw new InvalidOperationException("You cannot delete an inactive post");
            }

            if (!_author.Equals(username, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException($"This post does not belong to {username}");
            }

            RaiseEvent(new PostRemovedEvent { Id = _id });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }

    }
}