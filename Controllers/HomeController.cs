using System.Linq;
using System.Web.Mvc;
using PasswordHashingDemo.Data;
using PasswordHashingDemo.Models;
using PasswordHashingDemo.Util;

namespace PasswordHashingDemo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Signup()
        {
            return View("Signup");
        }

        [HttpPost]
        public ActionResult Signup(User user, string password)
        {
            if (ModelState.IsValid)
            {
                user.Password = Hashing.HashPasword(password);

                using (var session = NHibernateHelper.CreateSession())
                {
                    using (var txn = session.BeginTransaction())
                    {
                        session.Save(user);
                        txn.Commit();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(User user, string password)
        {
            string hashedPassword = Hashing.HashPasword(password);

            using (var session = NHibernateHelper.CreateSession())
            {
                var existingUser = session.Query<User>()
                  .FirstOrDefault(u => u.Username == user.Username && u.Password == hashedPassword);

                if (existingUser != null)
                {
                    //if user exists, redirect to Welcome page
                    return RedirectToAction("Welcome", new { username = existingUser.Username });
                }
                else
                {
                    //if credentials don't match, show error message
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View("Login", user);
                }
            }
        }
        public ActionResult Welcome(string username)
        {
            //pass the username to the view
            ViewBag.Username = username;
            return View("Welcome");
        }


    }
}