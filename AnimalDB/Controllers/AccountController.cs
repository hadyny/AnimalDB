using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Interfaces;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManagementService _users;

        public AccountController(IUserManagementService users)
        {
            _users = users;
        }
        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Models.LoginViewModel model, string returnUrl)
        {
            UserType? userType = await _users.GetUserType(model.UserName);

            if (userType == null)
            {
                ModelState.AddModelError("", "The account for user " + model.UserName + " has not been set up");
            }

            if (ModelState.IsValid)
            {
                if (await _users.SignInADUserAsync(model.UserName, model.Password, userType.Value))
                {
                    if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return this.Redirect(returnUrl);
                    }

                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}