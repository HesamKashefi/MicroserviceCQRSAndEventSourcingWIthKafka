using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess;
public class DatabaseContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<PostEntity> Posts { get; init; }
    public DbSet<CommentEntity> Comments { get; init; }
}
