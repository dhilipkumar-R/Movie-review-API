using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoRepository.Persistence.Repositories;

namespace MovieReviewApp.Repository
{
    public class CaseRepository : BaseMongoRepository<Movie>
    {
        private const string CaseCollectionName = "Cases_app";
        private readonly MongoDataContext _dataContext;

        public CaseRepository(MongoDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected override IMongoCollection<Movie> Collection => _dataContext.MongoDatabase.GetCollection<Movie>(CaseCollectionName);

        public virtual async Task<Movie> SaveAsync(Movie entity)
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
