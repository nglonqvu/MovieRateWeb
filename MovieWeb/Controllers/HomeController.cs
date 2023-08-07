using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CenimaDBContext _db;

        public HomeController(ILogger<HomeController> logger, CenimaDBContext db)
        {
            _logger = logger;
            this._db = db;
        }

        [HttpPost]
        public IActionResult Rate(Rate rate)
        {

            var PersonLogin = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("PersonLogin"));
            rate.PersonId = PersonLogin.PersonId;
            rate.Time = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.Rates.Add(rate);
                _db.SaveChanges();
            }
            var r = _db.Rates.Where(r => r.MovieId == rate.MovieId).ToList();

            foreach (Rate r1 in r)
            {
                r1.Person = _db.Persons.Find(r1.PersonId);
            }
            ViewData["Rates"] = r;

            ViewData["PersonID"] = rate.PersonId;

            var m = _db.Movies.Include(m => m.Genre).FirstOrDefault(m => m.MovieId == rate.MovieId);
            if (m != null)
            {
                ViewBag.MovieID = m.MovieId;
                ViewBag.Title = m.Title;
                ViewBag.Year = m.Year;
                ViewBag.Genre = m.Genre.Description;
                ViewBag.Description = m.Description;
                ViewBag.AverageRating = m.AverageRate();
            }
            ViewData["CurrentRate"] = r.FirstOrDefault(r => r.PersonId == rate.PersonId);
            ViewData["Movie"] = m;
            return View("Detail");
        }

        public IActionResult List(int id)
        {

            var movies = _db.Movies.Include(m => m.Genre).Include(m => m.Rates).ToList();
            if (id != 0)
            {
                movies = movies.Where(m => m.Genre.GenreId == id).ToList();
            }
            ViewData["movies"] = movies;

            ViewData["size"] = movies.Count;

            var genres = _db.Genres.ToList();
            ViewData["genres"] = genres;
            return View();
        }

        public IActionResult Detail(int id)
        {
            var m = _db.Movies.Include(m => m.Rates).Include(m => m.Genre).FirstOrDefault(m => m.MovieId == id);
            if (m != null)
            {
                ViewBag.MovieID = m.MovieId;
                ViewBag.Title = m.Title;
                ViewBag.Year = m.Year;
                ViewBag.Genre = m.Genre.Description;
                ViewBag.Description = m.Description;
                ViewBag.AverageRating = m.AverageRate();
            }
            ViewData["Movie"] = m;
            var r = _db.Rates.Where(r => r.MovieId == id).ToList();

            foreach (Rate rate in r)
            {
                rate.Person = _db.Persons.Find(rate.PersonId);

            }
            ViewData["Rates"] = r;


            Person PersonLogin;
            if (HttpContext.Session.GetString("PersonLogin") != null)
            {
                PersonLogin = (Person)JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("PersonLogin"));
                ViewData["PersonId"] = PersonLogin.PersonId;
                var currentRate = _db.Rates.Where(r => r.PersonId == PersonLogin.PersonId).FirstOrDefault(r => r.MovieId == id);
                if (currentRate != null)
                {
                    ViewData["CurrentRate"] = currentRate;
                }
            }
            else
            {
                ViewData["PersonId"] = 0;

            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Search(String value)
        {
            List<Movie> movies = _db.Movies.Include(m => m.Genre).Include(m => m.Rates).Where(m => m.Title.Contains(value)).ToList();
            ViewData["movies"] = movies;

            var genres = _db.Genres.ToList();
            ViewData["genres"] = genres;
            return View("List");
        }

        public IActionResult EditComment(int personid, int movieid)
        {
            var currentRate = _db.Rates.FirstOrDefault(r => r.MovieId == movieid && r.PersonId == personid);

            return View(currentRate);
        }

        [HttpPost]
        public IActionResult EditComment(Rate rate)
        {
            var currentRate = _db.Rates.FirstOrDefault(r => r.MovieId == rate.MovieId && r.PersonId == rate.PersonId);
            if (ModelState.IsValid)
            {
                if (currentRate != null)
                {
                    currentRate.NumericRating = rate.NumericRating;
                    currentRate.Comment = rate.Comment;
                    currentRate.Time = DateTime.Now;
                    _db.Update(currentRate);
                    _db.SaveChanges();
                }
                var r = _db.Rates.Where(r => r.MovieId == rate.MovieId).ToList();

                foreach (Rate r1 in r)
                {
                    r1.Person = _db.Persons.Find(r1.PersonId);
                }
                ViewData["Rates"] = r;

                ViewData["CurrentRate"] = currentRate;

                ViewData["PersonID"] = rate.PersonId;

                var m = _db.Movies.Include(m => m.Genre).FirstOrDefault(m => m.MovieId == rate.MovieId);
                if (m != null)
                {
                    ViewBag.MovieID = m.MovieId;
                    ViewBag.Title = m.Title;
                    ViewBag.Year = m.Year;
                    ViewBag.Genre = m.Genre.Description;
                    ViewBag.Description = m.Description;
                    ViewBag.AverageRating = m.AverageRate();
                }
                ViewData["Movie"] = m;
                return View("Detail");
            }
            return View();
        }
    }
}