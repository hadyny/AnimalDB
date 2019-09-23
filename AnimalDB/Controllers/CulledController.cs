using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Investigator, Technician, Administrator")]
    public class CulledController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CulledController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Culled/
        public async Task<ActionResult> Index(int? id)
        {
            var animals = _unitOfWork.Animals.GetDeceasedAnimals();

            if (User.IsInRole("Investigator"))
            {
                var investigator = await _unitOfWork.Investigators.GetByUsername(User.Identity.Name);
                animals.Where(m => m.Investigator_Id == investigator.Id);
            }

            if (id == null)
            {
                List<int> years = new List<int>();

                foreach (var animal in animals)
                {
                    if (!years.Contains(animal.DeathDate.Value.Year))
                    {
                        years.Add(animal.DeathDate.Value.Year);
                    }
                }

                years.Sort((a, b) => -1* a.CompareTo(b));

                ViewBag.Years = years;

                return View("SelectCulledYear");
            }

            animals = animals.Where(m => m.DeathDate.Value.Year == id).OrderByDescending(m => m.DeathDate);
            ViewBag.Year = id.Value;

            return View(animals.ToList());
        }

        // GET: /Culled/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await _unitOfWork.Animals.GetById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }
    }
}
