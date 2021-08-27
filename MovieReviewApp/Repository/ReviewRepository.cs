using MongoDB.Driver;
using MongoRepository.Persistence.Repositories;
using MovieReviewApp.Models.utils;


namespace MovieReviewApp.Repository
{
    public class ReviewRepository : BaseMongoRepository<Review>
    {
        private const string CaseCollectionName = Constants.BaseReview;
        private readonly MongoDataContext _dataContext;

        public ReviewRepository(MongoDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected override IMongoCollection<Review> Collection => _dataContext.MongoDatabase.GetCollection<Review>(CaseCollectionName);


    }
}
