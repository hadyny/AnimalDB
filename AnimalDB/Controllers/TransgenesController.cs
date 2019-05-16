using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Services;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class TransgenesController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private ITransgeneService _transgenes;

        public TransgenesController(ITransgeneService transgenes)
        {
            this._transgenes = transgenes;
        }

        // GET: Transgenes
        public async Task<ActionResult> Index()
        {
            return View(await _transgenes.GetTransgenes());
        }

        // GET: Transgenes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transgenes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description")] Transgene transgene)
        {
            if (ModelState.IsValid)
            {
                await _transgenes.CreateTransgene(transgene);
                return RedirectToAction("Index");
            }

            return View(transgene);
        }

        // GET: Transgenes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transgene transgene = await _transgenes.GetTransgeneById(id.Value);
            if (transgene == null)
            {
                return HttpNotFound();
            }
            return View(transgene);
        }

        // POST: Transgenes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description")] Transgene transgene)
        {
            if (ModelState.IsValid)
            {
                await _transgenes.UpdateTransgene(transgene);
                return RedirectToAction("Index");
            }
            return View(transgene);
        }

        // GET: Transgenes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transgene transgene = await _transgenes.GetTransgeneById(id.Value);
            if (transgene == null)
            {
                return HttpNotFound();
            }
            return View(transgene);
        }

        // POST: Transgenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Transgene transgene = await _transgenes.GetTransgeneById(id);
            await _transgenes.DeleteTransgene(transgene);
            return RedirectToAction("Index");
        }
    }
}
