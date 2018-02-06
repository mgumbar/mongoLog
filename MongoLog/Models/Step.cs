using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MongoLog.Models
{
    public class Step
    {
        [BsonElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
        [BsonElement("label")]
        [JsonProperty("label")]
        public string Label { get; set; }
        [BsonElement("category")]
        [JsonProperty("category")]
        public string Category { get; set; }
        [BsonElement("sub_category")]
        [JsonProperty("sub_category")]
        public string SubCategory { get; set; }
        [BsonElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }
        //[BsonElement("events")]
        //[BsonRepresentation(BsonType.String)]
        public List<StepEvent> Events { get; set; }

        public Step(string name, string label, string category, string subCategory)
        {
            this.Name = name;
            this.Label = label;
            this.Category = category;
            this.SubCategory = subCategory;
            this.SubCategory = subCategory;
            this.Status = "primary";
        }
    }
}