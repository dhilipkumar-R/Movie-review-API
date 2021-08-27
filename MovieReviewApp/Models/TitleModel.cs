using MongoDB.Bson;
using MovieReviewApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.Models
{
    public class TitleModel
    {
        public Movie Movie { get; set; }
        public List<ReviewsList> reviewList { get; set; }
        public int rating { get; set; }
    }

    public class ReviewsList
    {
        public ObjectId id { get; set; }
        public string comments { get; set; }
        public string user { get; set; }
    }

    public class MyReviewList
    {
        public string comments { get; set; }
        public string movie { get; set; }
    }
}
