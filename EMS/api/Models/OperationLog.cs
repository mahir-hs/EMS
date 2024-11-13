using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class OperationLog
    {
        [BsonId]
        public ObjectId Id { get; set; } 
        public required string OperationType { get; set; } 
        public required string EntityName { get; set; } 
        public required int EntityId { get; set; } 
        public required DateTime TimeStamp { get; set; } 
        public required string OperationDetails { get; set; } 
     

    }
}
