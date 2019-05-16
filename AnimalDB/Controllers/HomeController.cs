using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IAnimalService _animals;

        public HomeController(IAnimalService animals)
        {
            this._animals = animals;
        }

        // GET: /Home/
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("Investigator"))
            {
                ViewBag.animalid = new SelectList(await _animals.GetInvestigatorsAnimals(User.Identity.Name), "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Student"))
            {
                ViewBag.animalid = new SelectList(await _animals.GetStudentsAnimals(User.Identity.Name), "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Administrator") || 
                     User.IsInRole("Veterinarian") || 
                     User.IsInRole("Technician"))
            {
                ViewBag.animalid = new SelectList(await _animals.GetLivingAnimals(), "Id", "UniqueAnimalId");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string animalid)
        {
            Animal _animal = null;
            if (int.TryParse(animalid, out int value))
            {
                _animal = await _animals.GetAnimalById(value);
            }

            if (_animal != null)
            {
                return RedirectToAction("Details", "Animals", new { id = _animal.Id });
            }
            else
            {
                _animal = await _animals.GetAnimalByTag(animalid);
                if (_animal != null)
                {
                    return RedirectToAction("Details", "Animals", new { id = _animal.Id });
                }
            }

            if (User.IsInRole("Investigator"))
            {
                ViewBag.animalid = new SelectList(await _animals.GetInvestigatorsAnimals(User.Identity.Name), 
                                                "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Student"))
            {
                ViewBag.animalid = new SelectList(await _animals.GetStudentsAnimals(User.Identity.Name), 
                                                "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Administrator") || 
                     User.IsInRole("Veterinarian") || 
                     User.IsInRole("Technician"))
            {
                ViewBag.animalid = new SelectList(await _animals.GetLivingAnimals(), "Id", "UniqueAnimalId");
            }
            ViewBag.Errors = "Could not find the animal, please try again.";
            return View();
        }

        // /Home/Help
        public ActionResult Help()
        {
            return View();
        }

        public ActionResult AllergyInfo()
        {
            return View();
        }
	}
}