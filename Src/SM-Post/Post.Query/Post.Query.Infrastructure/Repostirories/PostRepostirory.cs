using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repostirories;
public class PostRepostirory(DatabaseContextFactory databaseContextFactory) : IPostRepository
{
    private readonly DatabaseContextFactory _databaseContextFactory = databaseContextFactory;

    public async Task CreateAsync(PostEntity postEntity)
    {
        using var db = _databaseContextFactory.CreateDbContext();
        db.Posts.Add(postEntity);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(PostEntity postEntity)
    {
        using var db = _databaseContextFactory.CreateDbContext();
        db.Posts.Update(postEntity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(PostEntity postEntity)
    {
        using var db = _databaseContextFactory.CreateDbContext();
        var post = await db.Posts.FirstOrDefaultAsync(x => x.PostId == postEntity.PostId);

        if (post is null) return;

        db.Posts.Remove(post);
        await db.SaveChangesAsync();
    }

    public async Task<PostEntity?> GetByIdAsync(Guid postId)
    {
        using var db = _databaseContextFactory.CreateDbContext();

        return await db.Posts
            .Include(x => x.Comments)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PostId == postId);
    }

    public async Task<PostEntity[]> ListAllAsync()
    {
        using var db = _databaseContextFactory.CreateDbContext();

        return await db.Posts
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<PostEntity[]> ListByAuthorAsync(string author)
    {
        using var db = _databaseContextFactory.CreateDbContext();

        return await db.Posts
            .Where(x => x.Author.Contains(author))
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<PostEntity[]> ListByCommentsAsync()
    {
        using var db = _databaseContextFactory.CreateDbContext();

        return await db.Posts
            .Where(x => x.Comments.Any())
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<PostEntity[]> ListByLikesAsync(int numberOfLikes)
    {
        using var db = _databaseContextFactory.CreateDbContext();

        return await db.Posts
            .Where(x => x.Likes >= numberOfLikes)
            .Include(x => x.Comments)
            .AsNoTracking()
            .ToArrayAsync();
    }
}
