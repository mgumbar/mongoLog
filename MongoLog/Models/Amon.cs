using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MongoLog.Models
{
    [BsonIgnoreExtraElements]
    public class Amon
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement("client_key")]
        public string ClientKey { get; set; }
        //public string Entity { get; set; }
        //public string Filename { get; set; }
        [BsonElement("sender")]
        public string Sender { get; set; }
        [BsonIgnoreIfNull]
        //[BsonSerializer(typeof(MongoDB.Bson.Serialization.Attributes.o))]
        //[BsonRepresentation(BsonType.String)]
        [BsonElement("steps")]
        [BsonRepresentation(BsonType.String)]
        public List<Step> Steps { get; set; }
        [BsonElement("payload")]
        public string Payload { get; set; }
        [BsonElement("created_at")]
        public string CreatedAt { get; set; }
        [BsonElement("updated_at")]
        public string UpdatedAt { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

}