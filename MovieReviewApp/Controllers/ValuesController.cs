using Microsoft.AspNetCore.Mvc;
using MovieReviewApp.IManagers;
using MovieReviewApp.Models;
using MovieReviewApp.Models.utils;
using System;

namespace MovieReviewApp.Controllers
{
    /// <summary>
    /// value Controller
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : Controller
    {
        private readonly IValues _values;

        public ValuesController(IValues values)
        {
            _values = values;
        }

        /// <summary>
        /// SaveMovieName
        /// </summary>
        /// <returns>Handler</returns>

        [HttpPost]
        public Handler SaveMovieName( [FromForm] string formObject)
        {
            try {
                 return _values.SaveMovieName(formObject);
            }
            catch(Exception ex){

                return new Handler() { Message = Constants.Exception, Exception = ex };
            }
           
        }

        /// <summary>
        /// GetMovieList
        /// </summary>
        /// <returns>JsonResult</returns>

        [HttpGet]
        public JsonResult GetMovieList(string username)
        {
            return  Json(_values.GetMovieList(username));
        }

        /// <summary>
        /// GetTitle
        /// </summary>
        /// <returns>JsonResult</returns>

        [HttpGet]
        public JsonResult GetTitle(string id, string user)
        {
            try
            {
                return Json(_values.GetTitle(id, user));
            }
            catch (Exception ex)
            {
                return Json(Constants.Exception);
            }

        }

        /// <summary>
        /// save Title
        /// </summary>
        /// <returns>Handler</returns>

        [HttpPost]
        public Handler SaveTitle([FromForm] string formObject, [FromForm] string deletedIds, string rating,string id, string username)
        {
            try
            {
                return _values.SaveTitle(formObject, deletedIds, rating, id, username);
            }
            catch (Exception ex)
            {

                return new Handler() { Message = Constants.Exception, Exception = ex };
            }

        }

        /// <summary>
        /// save input
        /// </summary>
        /// <returns>Handler</returns>

        [HttpGet]
        public JsonResult GetMyreviews(string username)
        {
            try
            {
                return Json(_values.GetMyreviews(username));
            }
            catch (Exception ex)
            {
                return Json(Constants.Exception);
            }
        }

    }
}
