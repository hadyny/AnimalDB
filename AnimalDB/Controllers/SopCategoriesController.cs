using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class SopCategoriesController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        ISopCategory _sopCategories;

        public SopCategoriesController()
        {
            this._sopCategories = new SopCategoryRepo();
        }

        // GET: SopCategories
        public ActionResult Index()
        {
            return View(_sopCategories.GetSopCategories());
        }

        // GET: SopCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SopCategory sopCategory = await _sopCategories.GetSopCategoryById(id.Value);
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
                await _sopCategories.CreateSopCategory(sopCategory);
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
            SopCategory sopCategory = await _sopCategories.GetSopCategoryById(id.Value);
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
                await _sopCategories.UpdateSopCategory(sopCategory);
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
            SopCategory sopCategory = await _sopCategories.GetSopCategoryById(id.Value);
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
            SopCategory sopCategory = await _sopCategories.GetSopCategoryById(id);
            await _sopCategories.DeleteSopCategory(sopCategory);
            return RedirectToAction("Index");
        }
    }
}
