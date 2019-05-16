using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class SpeciesController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        ISpeciesService _species;

        public SpeciesController(ISpeciesService species)
        {
            this._species = species;
        }

        // GET: /Species/
        public async Task<ActionResult> Index()
        {
            return View(await _species.GetSpecies());
        }

        // GET: /Species/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Species/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description")] Species species)
        {
            if (ModelState.IsValid)
            {
                await _species.CreateSpecies(species);
                return RedirectToAction("Index");
            }

            return View(species);
        }

        // GET: /Species/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = await _species.GetSpeciesById(id.Value);
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }

        // POST: /Species/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description")] Species species)
        {
            if (ModelState.IsValid)
            {
                await _species.UpdateSpecies(species);
                return RedirectToAction("Index");
            }
            return View(species);
        }

        // GET: /Species/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = await _species.GetSpeciesById(id.Value);
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }

        // POST: /Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Species species = await _species.GetSpeciesById(id);
            await _species.DeleteSpecies(species);
            return RedirectToAction("Index");
        }
    }
}
