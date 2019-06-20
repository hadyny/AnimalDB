using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class SourceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SourceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Source/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Sources.Get());
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
                _unitOfWork.Sources.Insert(source);
                await _unitOfWork.Complete();
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
            Source source = await _unitOfWork.Sources.GetById(id.Value);
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
                _unitOfWork.Sources.Update(source);
                await _unitOfWork.Complete();
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
            Source source = await _unitOfWork.Sources.GetById(id.Value);
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
            Source source = await _unitOfWork.Sources.GetById(id);
            _unitOfWork.Sources.Delete(source);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
