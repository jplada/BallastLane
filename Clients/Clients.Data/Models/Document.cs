using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Clients.Data.Models
{
    /// <summary>
    /// Base class for MongoDb documents
    /// </summary>
    public abstract class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
