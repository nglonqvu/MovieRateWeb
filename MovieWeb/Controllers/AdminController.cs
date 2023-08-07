using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.Controllers
{
	public class AdminController : Controller
	{
		private readonly CenimaDBContext _db;
		public AdminController(CenimaDBContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				return View("Index");

			}
			
				return RedirectToAction("Login", "Account");
			
		}

		public IActionResult AboutUs()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				return View("AboutUs");
			}
			return RedirectToAction("Login", "Account");
		}

		public IActionResult ListMember()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var persons = _db.Persons.ToList();
				ViewData["Persons"] = persons;
				return View("ListMember");
			}
			return RedirectToAction("Login", "Account");
		}

		public IActionResult IsActive(int personid, bool active)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var person = _db.Persons.Find(personid);
				if (!active)
				{
					person.IsActive = true;
					_db.Update(person);
					_db.SaveChanges();
				}
				else
				{
					person.IsActive = false;
					_db.Update(person);
					_db.SaveChanges();
				}
				return RedirectToAction("ListMember");
			}
			return RedirectToAction("Login", "Account");

		}

		public IActionResult SearchMember(string value)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var persons = _db.Persons.Where(p => p.Fullname.Contains(value)).ToList();
				ViewData["Persons"] = persons;
				return View("ListMember");
			}
			return RedirectToAction("Login", "Account");
		}

		public IActionResult ListMovie()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var movies = _db.Movies.ToList();
				var genres = _db.Genres.ToList();
				ViewData["Movies"] = movies;
				ViewData["Genres"] = genres;
				return View("ListMovie");
			}
			return RedirectToAction("Login", "Account");
		}

		public IActionResult CreateMovie()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				ViewData["GenreId"] = new SelectList(_db.Genres, "GenreId", "Description");
				return View();
			}
			return RedirectToAction("Login", "Account");
		}

		[HttpPost]
		public IActionResult CreateMovie(Movie movie)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				if (ModelState.IsValid)
				{
					_db.Movies.Add(movie);
					_db.SaveChanges();
					return RedirectToAction("ListMovie");
				}
				else
				{
					ViewData["GenreId"] = new SelectList(_db.Genres, "GenreId", "Description");
					return View(movie);
				}
			}
			return RedirectToAction("Login", "Account");
		}

		public IActionResult SearchMovie(string value)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var movies = _db.Movies.Where(m => m.Title.Contains(value)).ToList();
				var genres = _db.Genres.ToList();
				ViewData["Movies"] = movies;
				ViewData["Genres"] = genres;
				return View("ListMovie");
			}
			return RedirectToAction("Login", "Account");
		}

		public IActionResult DeleteMovie(int id)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var movie = _db.Movies.Find(id);
				var rate = _db.Rates.FirstOrDefault(r => r.MovieId == id);
				if (rate == null)
				{
					if(movie != null)
					{
						_db.Movies.Remove(movie);
						_db.SaveChanges();
					}
					return RedirectToAction("ListMovie");
				}
				var movies = _db.Movies.ToList();
				var genres = _db.Genres.ToList();
				ViewData["Movies"] = movies;
				ViewData["Genres"] = genres;
				ViewData["msg"] = "You can not delete this movie because it has Rate";
				return View("ListMovie");
			}
			return RedirectToAction("Login", "Account");

		}

		public IActionResult ListGenres()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				
				var genres = _db.Genres.ToList();
				ViewData["Genres"] = genres;
				return View("ListGenres");

			}

			return RedirectToAction("Login", "Account");

		}

		public IActionResult EditMovie(int id)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				ViewData["GenreId"] = new SelectList(_db.Genres, "GenreId", "Description");
				var movie = _db.Movies.Find(id);
				ViewData["Movie"] = movie;
				return View("EditMovie");

			}
			return RedirectToAction("Login", "Account");


		}

		[HttpPost]
		public IActionResult EditMovie(Movie movie)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var m = _db.Movies.Find(movie.MovieId);
				if (m != null)
				{
					m.Title = movie.Title;
					m.Year = movie.Year;
					m.GenreId = movie.GenreId;
					m.Description = movie.Description;
					m.Image = movie.Image;
					_db.Movies.Update(m);
					_db.SaveChanges();
				}
				return RedirectToAction("ListMovie");

			}

			return RedirectToAction("Login", "Account");

		}

		public IActionResult CreateGenres()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				ViewData["GenreId"] = new SelectList(_db.Genres, "GenreId", "Description");
				return View();

			}
			return RedirectToAction("Login", "Account");


		}

		[HttpPost]
		public IActionResult CreateGenres(Genre genre)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null)
			{
				if (ModelState.IsValid)
				{
					_db.Genres.Add(genre);
					_db.SaveChanges();
					return RedirectToAction("ListGenres");
				}
				else
				{
					return View(genre);
				}

			}
			return RedirectToAction("Login", "Account");


		}

		public IActionResult SearchGenres(string value)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{

				var genres = _db.Genres.Where(g => g.Description.Contains(value)).ToList();
				ViewData["Genres"] = genres;
				return View("ListGenres");
			}
			return RedirectToAction("Login", "Account");



		}

		public IActionResult EditGenre(int id)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{

				var genre = _db.Genres.Find(id);
				ViewData["Genre"] = genre;
				return View("EditGenre");
			}
			return RedirectToAction("Login", "Account");

		}

		[HttpPost]
		public ActionResult EditGenre(Genre genre)
		{
			if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
			{
				var g = _db.Genres.Find(genre.GenreId);
				if (g != null)
				{
					g.Description = genre.Description;
					_db.Update(g);
					_db.SaveChanges();
				}
				return RedirectToAction("ListGenres");

			}

			return RedirectToAction("Login", "Account");

		}

		public IActionResult DeleteGenre(int id)
		{

            if (HttpContext.Session.GetString("PersonLogin") != null && HttpContext.Session.GetString("Type").Equals("1"))
            {
                var genre = _db.Genres.Find(id);
                if (genre != null)
                {
                    var moviesToDelete = _db.Movies.Where(m => m.GenreId == id).ToList();

                    if (moviesToDelete.Count > 0)
                    {
                        foreach (var movie in moviesToDelete)
                        {
                            var rate = _db.Rates.FirstOrDefault(r => r.MovieId == movie.MovieId);
                            if (rate != null)
                            {
                                _db.Rates.Remove(rate);
                            }
                        }
                        _db.Movies.RemoveRange(moviesToDelete);
                    }
                    _db.Genres.Remove(genre);
                    _db.SaveChanges();
                }

                return RedirectToAction("ListGenres");
                var genres = _db.Genres.ToList();
                ViewData["Genres"] = genres;
                return View("ListGenres");
            }

            return RedirectToAction("Login", "Account");
        }



		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		//}
	}
}