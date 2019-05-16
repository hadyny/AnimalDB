using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
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
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IAnimalService _animals;
        private readonly IInvestigatorService _investigators;

        public CulledController(IAnimalService animals, IInvestigatorService investigators)
        {
            this._animals = animals;
            this._investigators = investigators;
        }

        // GET: /Culled/
        public async Task<ActionResult> Index(int? id)
        {
            var animals = await _animals.GetDeceasedAnimals();

            if (User.IsInRole("Investigator"))
            {
                var investigator = await _investigators.GetInvestigatorByUsername(User.Identity.Name);
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

                years.Sort();

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
            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }
    }
}
