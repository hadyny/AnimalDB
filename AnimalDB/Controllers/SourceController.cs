using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class SourceController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        ISource _sources;

        public SourceController()
        {
            this._sources = new SourceRepo();
        }


        // GET: /Source/
        public ActionResult Index()
        {
            return View(_sources.GetSources());
        }

        // GET: /Source/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Source/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description,Type")] Source source)
        {
            if (ModelState.IsValid)
            {
                await _sources.CreateSource(source);
                return RedirectToAction("Index");
            }

            return View(source);
        }

        // GET: /Source/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = await _sources.GetSourceById(id.Value);
            if (source == null)
            {
                return HttpNotFound();
            }
            return View(source);
        }

        // POST: /Source/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description,Type")] Source source)
        {
            if (ModelState.IsValid)
            {
                await _sources.UpdateSource(source);
                return RedirectToAction("Index");
            }
            return View(source);
        }

        // GET: /Source/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = await _sources.GetSourceById(id.Value);
            if (source == null)
            {
                return HttpNotFound();
            }
            return View(source);
        }

        // POST: /Source/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Source source = await _sources.GetSourceById(id);
            await _sources.DeleteSource(source);
            return RedirectToAction("Index");
        }
    }
}
