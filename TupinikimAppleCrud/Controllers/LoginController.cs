using Microsoft.AspNetCore.Mvc;
using Firebase.Auth;
using Newtonsoft.Json;
using static TupinikimAppleCrud.Models.LoginModel;
using TupinikimAppleCrud.Models;

namespace TupinikimAppleCrud.Controllers
{
    public class LoginController : Controller
    {
        FirebaseAuthProvider auth;
        public LoginController()
        {
            auth = new FirebaseAuthProvider(
                            new FirebaseConfig("AIzaSyBwDlWOAT8RhurOPW4MbzvrRC5yeGSkQ50"));
        }

        [HttpPost]
        public async Task<IActionResult> Registration(LoginModel loginModel)
        {
            try
            {
                //create the user
                await auth.CreateUserWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                //log in the new user
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //saving the token in a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);

                    return RedirectToAction("Index");
                }
            }
            catch (FirebaseAuthException ex)
            {
                //var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                //ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                //return View(loginModel);
            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel loginModel)
        {
            try
            {
                //log in an existing user
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //save the token to a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);

                    return RedirectToAction("Index");
                }

            }
            catch (FirebaseAuthException ex)
            {
                //var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                //ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                //return View(loginModel);
            }

            return View("Index");
        }

        public IActionResult Index()
        {
            return View("Login");
        }
    }
}
