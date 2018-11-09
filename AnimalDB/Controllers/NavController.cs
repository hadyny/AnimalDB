using AnimalDB.Functions;
using AnimalDB.Models;
using System.Web.Mvc;

namespace AnimalDB.Web.Controllers
{
    public class NavController : Controller
    {
        public PartialViewResult Menu()
        {
            MenuViewModel model = HelperFunctions.CreateNavStatus(User.Identity.Name);
            return PartialView(model);
        }
    }
}