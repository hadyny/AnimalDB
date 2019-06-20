using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class SpeciesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpeciesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Species/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Species.Get());
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
                _unitOfWork.Species.Insert(species);
                await _unitOfWork.Complete();
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
            Species species = await _unitOfWork.Species.GetById(id.Value);
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
                _unitOfWork.Species.Update(species);
                await _unitOfWork.Complete();
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
            Species species = await _unitOfWork.Species.GetById(id.Value);
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
            Species species = await _unitOfWork.Species.GetById(id);
            _unitOfWork.Species.Delete(species);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
