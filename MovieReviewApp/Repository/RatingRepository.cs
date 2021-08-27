using MongoDB.Driver;
using MongoRepository.Persistence.Repositories;
using MovieReviewApp.Models.utils;


namespace MovieReviewApp.Repository
{
    public class RatingRepository : BaseMongoRepository<Rating>
    {
        private const string CaseCollectionName = Constants.BaseRating;
        private readonly MongoDataContext _dataContext;

        public RatingRepository(MongoDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected override IMongoCollection<Rating> Collection => _dataContext.MongoDatabase.GetCollection<Rating>(CaseCollectionName);

    }
}
