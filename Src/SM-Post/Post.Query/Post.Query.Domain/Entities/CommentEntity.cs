using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Post.Query.Domain.Entities;

public class CommentEntity
{
    [Key]
    public Guid CommentId { get; set; }
    public required string Username { get; set; }
    public required string Message { get; set; }
    public DateTime CommentDate { get; set; }

    public Guid PostId { get; private set; }

    [JsonIgnore]
    public virtual PostEntity? PostEntity { get; private set; }
}
