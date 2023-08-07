using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class AccountController : Controller
	{
		private readonly CenimaDBContext _db;

		public AccountController(CenimaDBContext db)
		{
			_db = db;
		}

		[BindProperty]
		public Person? Person { get; set; }

		public IActionResult Login()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null)
			{
				return RedirectToAction("List", "Home");
			}
			return View();
		}

		[HttpPost]
		public IActionResult Login(Person person)
		{
			Person = _db.Persons.Where(x => x.Email == person.Email & x.Password == person.Password).FirstOrDefault();
			if (Person == null)
			{
				Debug.WriteLine("Person null");
				ViewData["msg"] = "Email hoặc mật khẩu không đúng.";
				return View();
			}
			if (Person.IsActive == null)
			{
				Debug.WriteLine("Person null");
				ViewData["msg"] = "Tài khoản của bạn đã bị deactive.";
				return View();
			}

			HttpContext.Session.SetString("PersonLogin", JsonSerializer.Serialize(Person));
			HttpContext.Session.SetString("Type", Person.Type.ToString());
			Debug.WriteLine("Save to session");
			if (Person.Type == 1)
			{
				return RedirectToAction("Index", "Admin");
			}
			else
			{
				return RedirectToAction("List", "Home");
			}
		}

		public IActionResult Logout()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null)
				HttpContext.Session.Remove("PersonLogin");
			return RedirectToAction("List", "Home");
		}

		public IActionResult Signup()
		{
			if (HttpContext.Session.GetString("PersonLogin") != null)
			{
				return RedirectToAction("List", "Home");
			}
			return View("Views/Account/Signup.cshtml");
		}

		[HttpPost]
		public IActionResult Signup(Person person)
		{
			if (ModelState.IsValid)
			{
				Person = _db.Persons.Where(x => x.Email == person.Email).FirstOrDefault();

				if (Person != null)
				{
					ViewData["msg"] = "Email này đã tồn tại. Vui lòng lựa chọn email khác.";
					return View();
				}
				person.IsActive = true;
				person.Type = 2;

				_db.Persons.Add(person);
				_db.SaveChanges();

				ViewData["msg"] = "Tài khoản đăng ký thành công";
				return View("Login");
			}
			return View();
		}

	}
}
