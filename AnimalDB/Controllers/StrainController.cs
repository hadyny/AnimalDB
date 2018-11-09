using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class StrainController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IStrain _strains;
        private ISpecies _species;

        public StrainController()
        {
            this._strains = new StrainRepo();
            this._species = new SpeciesRepo();
        }

        // GET: /Strain/
        public ActionResult Index()
        {
            return View(_strains.GetStrains());
        }


        // GET: /Strain/Create
        public ActionResult Create()
        {
            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description");
            return View();
        }

        // POST: /Strain/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description,Species_Id,Code")] Strain strain)
        {
            if (ModelState.IsValid)
            {
                await _strains.CreateStrain(strain);
                return RedirectToAction("Index");
            }

            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description", strain.Species_Id);
            return View(strain);
        }

        // GET: /Strain/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Strain strain = await _strains.GetStrainById(id.Value);
            if (strain == null)
            {
                return HttpNotFound();
            }
            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description", strain.Species_Id);
            return View(strain);
        }

        // POST: /Strain/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description,Species_Id,Code")] Strain strain)
        {
            if (ModelState.IsValid)
            {
                await _strains.UpdateStrain(strain);
                return RedirectToAction("Index");
            }
            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description", strain.Species_Id);
            return View(strain);
        }

        // GET: /Strain/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Strain strain = await _strains.GetStrainById(id.Value);
            if (strain == null)
            {
                return HttpNotFound();
            }
            return View(strain);
        }

        // POST: /Strain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Strain strain = await _strains.GetStrainById(id);
            await _strains.DeleteStrain(strain);
            return RedirectToAction("Index");
        }
    }
}
