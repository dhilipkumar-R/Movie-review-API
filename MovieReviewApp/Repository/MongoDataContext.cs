using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.Repository
{
    public class MongoDataContext
    {
   
        public IMongoDatabase MongoDatabase { get; }
        public MongoDataContext()
            : this("MongoDbTests")
        {
        }

        public MongoDataContext(string connectionName)
        {
            try
            {
                var url = connectionName;
                var mongoUrl = new MongoUrl(url);
                IMongoClient client = new MongoClient(mongoUrl);
                MongoDatabase = client.GetDatabase(mongoUrl.DatabaseName);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
