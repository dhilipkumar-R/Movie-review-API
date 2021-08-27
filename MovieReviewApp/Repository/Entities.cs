using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.Repository
{
    public class Movie : IEntity
    {
        [BsonElement("Id")]
        public ObjectId Id { get; set; }
        [BsonElement("MovieName")]
        public string MovieName { get; set; }
        [BsonElement("year")]
        public string year { get; set; }
        [BsonElement("country")]
        public string country { get; set; }
        [BsonElement("language")]
        public string language { get; set; }
        [BsonElement("genre")]
        public string genre { get; set; }
        string IEntity<string>.Id { get; set; }
        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }


    public class Review : IEntity
    {
        [BsonElement("Id")]
        public ObjectId Id { get; set; }
        [BsonElement("CaseId")]
        public ObjectId CaseId { get; set; }
        [BsonElement("Comments")]
        public string Comments { get; set; }
        [BsonElement("MovieName")]
        public string MovieName { get; set; }
        [BsonElement("User")]
        public string User { get; set; }
        string IEntity<string>.Id { get; set; }
        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }

    public class Rating : IEntity
    {
        [BsonElement("Id")]
        public ObjectId Id { get; set; }
        [BsonElement("CaseId")]
        public ObjectId CaseId { get; set; }
        [BsonElement("Ratings")]
        public int Ratings { get; set; }
        [BsonElement("MovieName")]
        public string MovieName { get; set; }
        [BsonElement("User")]
        public string User { get; set; }
        string IEntity<string>.Id { get; set; }
        [BsonExtraElements]
        public BsonDocument CatchAll { get; set; }
    }
}
