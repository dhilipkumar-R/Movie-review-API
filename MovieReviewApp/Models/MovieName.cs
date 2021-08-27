using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.Models
{
    public class MovieName
    {
        public string movie { get; set; }
        public string year { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public string genre { get; set; }
    }

    public class GridList
    {
        public ObjectId id { get; set; }
        public string filmname { get; set; }
        public string year { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public string genre { get; set; }
        public string rating { get; set; }
    }
    public class Reviewlist
    {
     public string id { get; set; }
     public string comments { get; set; }
     public string user { get; set; }
     public bool isdeleted = false;
    }
    public class DeletedList
    {
        public string id { get; set; }
    }

}
