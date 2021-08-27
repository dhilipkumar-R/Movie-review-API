using MovieReviewApp.IManagers;
using System;
using Newtonsoft.Json;
using MovieReviewApp.Models;
using System.Linq;
using MongoDB.Bson;
using MovieReviewApp.Repository;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using MovieReviewApp.Models.utils;

namespace MovieReviewApp.Managers
{
    public class Values : IValues
    {
        private readonly CaseRepository _caseRepository;
        private readonly ReviewRepository _reviewRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly MongoDataContext context;

        private static string _connectionstring = string.Empty;

        public Values(IOptions<Configuration> settings)
        {
            _connectionstring = Crypto.DecryptStringAES(Constants.SecretKey , Constants.DBName);
            context = new MongoDataContext(_connectionstring);
            _caseRepository = new CaseRepository(context);
            _reviewRepository = new ReviewRepository(context);
            _ratingRepository = new RatingRepository(context);
        }
        #region "public methods"
        public Handler SaveMovieName(string formObject)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<MovieName>(formObject);

                Movie movie = new Movie
                {
                    Id = ObjectId.GenerateNewId(),
                    MovieName = data.movie,
                    language = data.language,
                    country = data.country,
                    genre = data.genre,
                    year = data.year
                };

                _caseRepository.SaveAsync(movie).GetAwaiter().GetResult();

                return new Handler() { Status = true, Message = Constants.SuccessMessage };

            }
            catch (Exception ex)
            {
                return new Handler() { Message = Constants.Exception, Exception = ex };

            }

        }

        public List<GridList> GetMovieList(string username)
        {
            try
            {
                var data = _caseRepository.GetAll().GetAwaiter().GetResult();


                List<GridList> gridLists = data.AsEnumerable().Select(x => new GridList()
                {
                    id = x.Id,
                    filmname = x.MovieName,
                    language = x.language,
                    country = x.country,
                    genre = x.genre,
                    year = x.year,
                    rating = CalculateWeightedRating(x)
                }).ToList();


                return gridLists.OrderByDescending(x => x.rating).ToList();

            }
            catch (Exception ex)
            {
                return null;

            }

        }

        public TitleModel GetTitle(string id, string user)
        {
            try
            {

                Movie data = _caseRepository.FindOneAsync(x => x.Id.Equals(new ObjectId(id))).GetAwaiter().GetResult();
                Rating rating = _ratingRepository.FindOneAsync(x => x.CaseId.Equals(new ObjectId(id)) && x.User.Equals(user)).GetAwaiter().GetResult();
                var reviews = _reviewRepository.FindAllAsync(x => x.CaseId.Equals(new ObjectId(id))).GetAwaiter().GetResult();

                TitleModel titleModel = new TitleModel
                {
                    Movie = new Movie
                    {
                        Id = data.Id,
                        MovieName = data.MovieName,
                        language = data.language,
                        genre = data.genre,
                        country = data.country,
                        year = data.year,
                    },
                };

                titleModel.reviewList = reviews.ToList().Select(x => new ReviewsList()
                {
                    id = x.Id,
                    comments = x.Comments,
                    user = x.User
                }).ToList();

                titleModel.rating = (rating != null) ? rating.Ratings : 0;
                return titleModel;

            }
            catch (Exception ex)
            {
                TitleModel titleModel = new TitleModel();
                return titleModel;

            }
        }

        public Handler SaveTitle(string formObject,string deletedIds, string rating, string id, string username)
        {
            try
            {

                var reviewdata = JsonConvert.DeserializeObject<List<Reviewlist>>(formObject);

                var data = _caseRepository.FindOneAsync(x => x.Id.Equals(new ObjectId(id))).GetAwaiter().GetResult();

                var deletedids = JsonConvert.DeserializeObject<List<Reviewlist>>(deletedIds);

 
                deletedids.ForEach((val) =>
                {
                    _reviewRepository.DeleteAsync(val.id.ToString()).GetAwaiter().GetResult();
                });

                reviewdata.ForEach((val) =>
                {
                    if (string.IsNullOrEmpty(val.id))
                    {
                        Review review = new Review
                        {
                            Id = ObjectId.GenerateNewId(),
                            CaseId = data.Id,
                            MovieName = data.MovieName,
                            Comments = val.comments,
                            User = username
                        };

                        _reviewRepository.SaveAsync(review).GetAwaiter().GetResult();
                    }
                    else
                    {
                        var reviews = _reviewRepository.FindAllAsync(x => x.CaseId.Equals(data.Id)).GetAwaiter().GetResult();

                            reviews.ToList().ForEach(x => {

                                if (x.Id == new ObjectId(val.id) && x.Comments != val.comments)
                                {
                                    Review review = new Review
                                    {
                                        Id = new ObjectId(val.id),
                                        CaseId = data.Id,
                                        MovieName = data.MovieName,
                                        Comments = val.comments,
                                        User = username
                                    };

                                    _reviewRepository.SaveAsync(review).GetAwaiter().GetResult();
                                }
                            });

                          }
                });



                var ratinglists = _ratingRepository.FindAllAsync(x => x.CaseId.Equals(data.Id)).GetAwaiter().GetResult();

                if (ratinglists != null && ratinglists.Count() > 0)
                {
                    ratinglists.ToList().ForEach(x => {

                        if (x.Ratings != (string.IsNullOrEmpty(rating) ? 0 : Convert.ToInt32(rating)) && x.User.Equals(username))
                        {
                            Rating ratingupd = new Rating
                            {
                                Id = x.Id,
                                MovieName = data.MovieName,
                                CaseId = data.Id,
                                Ratings = string.IsNullOrEmpty(rating) ? 0 : Convert.ToInt32(rating),
                                User = username
                            };

                            _ratingRepository.SaveAsync(ratingupd).GetAwaiter().GetResult();
                        }
                    });
                }


                    Rating ratings = new Rating
                    {
                        Id = ObjectId.GenerateNewId(),
                        CaseId = data.Id,
                        MovieName = data.MovieName,
                        Ratings = string.IsNullOrEmpty(rating) ? 0 : Convert.ToInt32(rating),
                        User = username
                    };

                    _ratingRepository.SaveAsync(ratings).GetAwaiter().GetResult();
           


                _caseRepository.SaveAsync(data).GetAwaiter().GetResult();

                return new Handler() { Status = true, Message = Constants.TitleSuccessMessage };

            }
            catch (Exception ex)
            {
                return new Handler() { Message = Constants.Exception, Exception = ex };

            }

        }

        public List<MyReviewList> GetMyreviews( string username)
        {
            try
            {

                var reviews = _reviewRepository.FindAllAsync(x => x.User.Equals(username)).GetAwaiter().GetResult();


                List<MyReviewList> myReviewLists = reviews.ToList().Select(x => new MyReviewList()
                {
                    comments = x.Comments,
                     movie = x.MovieName
                }).ToList();

                return myReviewLists;

            }
            catch (Exception ex)
            {
                return null;

            }
        }
        #endregion
        #region "private methods"
        private string CalculateWeightedRating(Movie gridList)
        {
           int count = 0;
           int totalsumratings = 0;


           var rating = _ratingRepository.FindAllAsync(z => z.CaseId.Equals(gridList.Id)).GetAwaiter().GetResult();

            if (rating != null && rating.Count() > 0)
            {
                rating.ToList().ForEach(y =>
                {
                    count = count + 1;
                    totalsumratings = totalsumratings + y.Ratings;
                });

                string total = string.Format("{0:0.0}", totalsumratings);
                return (Decimal.Parse(total) / count).ToString();
            }
            else
            {
                return string.Format("{0:0.0}", totalsumratings);
            }

        }
        #endregion
    }

}
