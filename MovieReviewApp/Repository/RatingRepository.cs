using MongoDB.Driver;
using MongoRepository.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.Repository
{
    public class RatingRepository : BaseMongoRepository<Rating>
    {
        private const string CaseCollectionName = "Cases_rating";
        private readonly MongoDataContext _dataContext;

        public RatingRepository(MongoDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected override IMongoCollection<Rating> Collection => _dataContext.MongoDatabase.GetCollection<Rating>(CaseCollectionName);

        public virtual async Task<Rating> SaveAsync(Rating entity)
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
