using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DocumentsAPI.Models
{
    public class Photo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Url")]
        public string Url { get; set; } = null!;
    }
}
