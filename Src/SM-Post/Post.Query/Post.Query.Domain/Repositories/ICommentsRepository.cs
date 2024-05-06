using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;

public interface ICommentsRepository
{
    Task CreateAsync(CommentEntity commentEntity);
    Task UpdateAsync(CommentEntity commentEntity);
    Task DeleteAsync(CommentEntity commentEntity);

    Task<PostEntity> GetByIdAsync(Guid commentId);
}
