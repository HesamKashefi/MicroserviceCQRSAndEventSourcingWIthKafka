using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repostirories;

public class CommentsRepostirory(DatabaseContextFactory databaseContextFactory) : ICommentsRepository
{
    private readonly DatabaseContextFactory _databaseContextFactory = databaseContextFactory;

    public async Task CreateAsync(CommentEntity commentEntity)
    {
        using var db = _databaseContextFactory.CreateDbContext();
        db.Comments.Add(commentEntity);
        await db.SaveChangesAsync();
    }
    public async Task UpdateAsync(CommentEntity commentEntity)
    {
        using var db = _databaseContextFactory.CreateDbContext();
        db.Comments.Update(commentEntity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(CommentEntity commentEntity)
    {
        using var db = _databaseContextFactory.CreateDbContext();

        var comment = await db.Comments.FirstOrDefaultAsync(x => x.CommentId == commentEntity.CommentId);

        if (comment is null) return;

        db.Comments.Remove(comment);
        await db.SaveChangesAsync();
    }

    public async Task<CommentEntity?> GetByIdAsync(Guid commentId)
    {
        using var db = _databaseContextFactory.CreateDbContext();

        return await db.Comments.FirstOrDefaultAsync(x => x.CommentId == commentId);
    }
}
