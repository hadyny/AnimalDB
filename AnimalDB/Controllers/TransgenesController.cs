using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class TransgenesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransgenesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Transgenes
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Transgenes.Get());
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
                _unitOfWork.Transgenes.Insert(transgene);
                await _unitOfWork.Complete();
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
            Transgene transgene = await _unitOfWork.Transgenes.GetById(id.Value);
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
                _unitOfWork.Transgenes.Update(transgene);
                await _unitOfWork.Complete();
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
            Transgene transgene = await _unitOfWork.Transgenes.GetById(id.Value);
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
            Transgene transgene = await _unitOfWork.Transgenes.GetById(id);
            _unitOfWork.Transgenes.Delete(transgene);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
