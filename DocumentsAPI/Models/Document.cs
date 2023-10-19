using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DocumentsAPI.Models
{
    public class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Url")]
        public string Url { get; set; } = null!;


        //redundancy
        [BsonElement("Complaints")]
        public string Complaints { get; set; } = null!;

        [BsonElement("Conclusion")]
        public string Conclusion { get; set; } = null!;

        [BsonElement("Recommendations")]
        public string Recommendations { get; set; } = null!;

        [BsonElement("AppointmentId")]
        public int AppointmentId { get; set; }
    }
}
