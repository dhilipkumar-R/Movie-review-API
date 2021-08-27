using MongoDB.Driver;
using MongoRepository.Persistence.Repositories;
using MovieReviewApp.Models.utils;

namespace MovieReviewApp.Repository
{
    public class CaseRepository : BaseMongoRepository<Movie>
    {
        private const string CaseCollectionName = Constants.BaseCase;
        private readonly MongoDataContext _dataContext;

        public CaseRepository(MongoDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected override IMongoCollection<Movie> Collection => _dataContext.MongoDatabase.GetCollection<Movie>(CaseCollectionName);

    }
}
