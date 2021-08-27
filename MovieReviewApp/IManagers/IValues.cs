using MovieReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewApp.IManagers
{
    public interface IValues
    {
        Handler SaveMovieName(string formObject);
        List<GridList> GetMovieList(string username);
        TitleModel GetTitle(string id, string user);
        Handler SaveTitle(string formObject, string deletedIds, string rating, string id, string username);
        List<MyReviewList> GetMyreviews(string username);
    }
}
