using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class VirusTypesController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IVirusType _virusTypes;

        public VirusTypesController()
        {
            this._virusTypes = new VirusTypeRepo();
        }

        // GET: VirusTypes
        public ActionResult Index()
        {
            return View(_virusTypes.GetVirusTypes());
        }

        // GET: VirusTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VirusTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description")] VirusType virusType)
        {
            if (ModelState.IsValid)
            {
                await _virusTypes.CreateVirusType(virusType);
                return RedirectToAction("Index");
            }

            return View(virusType);
        }

        // GET: VirusTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirusType virusType = await _virusTypes.GetVirusTypeById(id.Value);
            if (virusType == null)
            {
                return HttpNotFound();
            }
            return View(virusType);
        }

        // POST: VirusTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description")] VirusType virusType)
        {
            if (ModelState.IsValid)
            {
                await _virusTypes.UpdateVirusType(virusType);
                return RedirectToAction("Index");
            }
            return View(virusType);
        }

        // GET: VirusTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirusType virusType = await _virusTypes.GetVirusTypeById(id.Value);
            if (virusType == null)
            {
                return HttpNotFound();
            }
            return View(virusType);
        }

        // POST: VirusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VirusType virusType = await _virusTypes.GetVirusTypeById(id);
            await _virusTypes.DeleteVirusType(virusType);
            return RedirectToAction("Index");
        }
    }
}
