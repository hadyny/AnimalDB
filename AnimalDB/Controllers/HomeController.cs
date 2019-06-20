using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Home/
        public ActionResult Index()
        {
            if (User.IsInRole("Investigator"))
            {
                ViewBag.animalid = new SelectList(_unitOfWork.Animals.GetInvestigatorsAnimals(User.Identity.Name), "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Student"))
            {
                ViewBag.animalid = new SelectList(_unitOfWork.Animals.GetStudentsAnimals(User.Identity.Name), "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Administrator") || 
                     User.IsInRole("Veterinarian") || 
                     User.IsInRole("Technician"))
            {
                ViewBag.animalid = new SelectList(_unitOfWork.Animals.GetLiving(), "Id", "UniqueAnimalId");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string animalid)
        {
            Animal _animal = null;
            if (int.TryParse(animalid, out int value))
            {
                _animal = await _unitOfWork.Animals.GetById(value);
            }

            if (_animal != null)
            {
                return RedirectToAction("Details", "Animals", new { id = _animal.Id });
            }
            else
            {
                _animal = _unitOfWork.Animals.GetAnimalByTag(animalid);
                if (_animal != null)
                {
                    return RedirectToAction("Details", "Animals", new { id = _animal.Id });
                }
            }

            if (User.IsInRole("Investigator"))
            {
                ViewBag.animalid = new SelectList(_unitOfWork.Animals.GetInvestigatorsAnimals(User.Identity.Name), 
                                                "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Student"))
            {
                ViewBag.animalid = new SelectList(_unitOfWork.Animals.GetStudentsAnimals(User.Identity.Name), 
                                                "Id", "UniqueAnimalId");
            }
            else if (User.IsInRole("Administrator") || 
                     User.IsInRole("Veterinarian") || 
                     User.IsInRole("Technician"))
            {
                ViewBag.animalid = new SelectList(_unitOfWork.Animals.GetLiving(), "Id", "UniqueAnimalId");
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