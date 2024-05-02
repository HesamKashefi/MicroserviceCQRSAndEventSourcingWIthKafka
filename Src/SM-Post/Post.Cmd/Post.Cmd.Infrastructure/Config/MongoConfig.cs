namespace Post.Cmd.Infrastructure.Config
{
    public class MongoConfig
    {
        public required string ConnnectionString { get; init; }
        public required string Database { get; init; }
        public required string Collection { get; init; }
    }
}