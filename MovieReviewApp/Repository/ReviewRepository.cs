using MongoDB.Driver;
using MongoRepository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.Repository
{
    public class ReviewRepository : BaseMongoRepository<Review>
    {
        private const string CaseCollectionName = "Cases_review";
        private readonly MongoDataContext _dataContext;

        public ReviewRepository(MongoDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected override IMongoCollection<Review> Collection => _dataContext.MongoDatabase.GetCollection<Review>(CaseCollectionName);

        public virtual async Task<Review> SaveAsync(Review entity)
        {

            await Collection.ReplaceOneAsync(
                x => x.Id.Equals(entity.Id),
                entity,
                new UpdateOptions
                {
                    IsUpsert = true
                });

            return entity;
        }
    }
}
