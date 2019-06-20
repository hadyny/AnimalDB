using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class SopCategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SopCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: SopCategories
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.SopCategories.Get());
        }

        // GET: SopCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SopCategory sopCategory = await _unitOfWork.SopCategories.GetById(id.Value);
            if (sopCategory == null)
            {
                return HttpNotFound();
            }
            return View(sopCategory);
        }

        [Authorize(Roles = "Administrator,Technician")]
        // GET: SopCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SopCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description")] SopCategory sopCategory)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SopCategories.Insert(sopCategory);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(sopCategory);
        }

        [Authorize(Roles = "Administrator,Technician")]
        // GET: SopCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SopCategory sopCategory = await _unitOfWork.SopCategories.GetById(id.Value);
            if (sopCategory == null)
            {
                return HttpNotFound();
            }
            return View(sopCategory);
        }

        // POST: SopCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description")] SopCategory sopCategory)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SopCategories.Update(sopCategory);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(sopCategory);
        }

        [Authorize(Roles = "Administrator,Technician")]
        // GET: SopCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SopCategory sopCategory = await _unitOfWork.SopCategories.GetById(id.Value);
            if (sopCategory == null)
            {
                return HttpNotFound();
            }
            return View(sopCategory);
        }

        // POST: SopCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SopCategory sopCategory = await _unitOfWork.SopCategories.GetById(id);
            _unitOfWork.SopCategories.Delete(sopCategory);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
