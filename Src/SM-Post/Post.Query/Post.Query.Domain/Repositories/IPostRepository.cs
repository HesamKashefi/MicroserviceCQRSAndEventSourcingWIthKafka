using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories;
public interface IPostRepository
{
    Task CreateAsync(PostEntity postEntity);
    Task UpdateAsync(PostEntity postEntity);
    Task DeleteAsync(PostEntity postEntity);

    Task<PostEntity?> GetByIdAsync(Guid postId);
    Task<PostEntity[]> ListAllAsync();
    Task<PostEntity[]> ListByAuthorAsync(string author);
    Task<PostEntity[]> ListByLikesAsync(int numberOfLikes);
    Task<PostEntity[]> ListByCommentsAsync();
}
