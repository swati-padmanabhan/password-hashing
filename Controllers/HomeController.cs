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

        //displays the signup view
        public ActionResult Signup()
        {
            return View("Signup");
        }

        //signup - handles form submission for user signup
        [HttpPost]
        public ActionResult Signup(User user, string password)
        {
            //check if the model state is valid (e.g., required fields are filled correctly)
            if (ModelState.IsValid)
            {
                //hash the password before saving the user
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

        //displays the login view
        public ActionResult Login()
        {
            return View("Login");
        }

        //login - handles form submission for user login
        [HttpPost]
        public ActionResult Login(User user, string password)
        {
            //hash the provided password for comparison
            string hashedPassword = Hashing.HashPasword(password);

            using (var session = NHibernateHelper.CreateSession())
            {
                //query the database to find a user with the provided username
                var existingUser = session.Query<User>()
                .FirstOrDefault(u => u.Username == user.Username);

                //check if the user exists and if the provided password matches the stored hashed password

                if (existingUser != null && Hashing.ValidatePassword(password, existingUser.Password))
                {
                    //redirect to welcome page
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

        //displays the welcome view
        public ActionResult Welcome(string username)
        {
            //pass the username to the view
            ViewBag.Username = username;
            return View("Welcome");
        }


    }
}