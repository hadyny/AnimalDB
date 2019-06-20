using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class VirusTypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VirusTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: VirusTypes
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.VirusTypes.Get());
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
                _unitOfWork.VirusTypes.Insert(virusType);
                await _unitOfWork.Complete();
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
            VirusType virusType = await _unitOfWork.VirusTypes.GetById(id.Value);
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
                _unitOfWork.VirusTypes.Update(virusType);
                await _unitOfWork.Complete();
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
            VirusType virusType = await _unitOfWork.VirusTypes.GetById(id.Value);
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
            VirusType virusType = await _unitOfWork.VirusTypes.GetById(id);
            _unitOfWork.VirusTypes.Delete(virusType);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
