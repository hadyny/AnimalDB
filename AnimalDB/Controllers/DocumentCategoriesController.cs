using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Web.Controllers
{
    [Authorize]
    public class DocumentCategoriesController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IDocumentCategoryService _categories;

        public DocumentCategoriesController(IDocumentCategoryService categories)
        {
            this._categories = categories;
        }

        // GET: DocumentCategories
        public async Task<ActionResult> Index()
        {
            return View(await _categories.GetDocumentCategories());
        }

        // GET: DocumentCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description")] DocumentCategory documentCategory)
        {
            if (ModelState.IsValid)
            {
                await _categories.CreateDocumentCategory(documentCategory);
                return RedirectToAction("Index");
            }

            return View(documentCategory);
        }

        // GET: DocumentCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentCategory documentCategory = await _categories.GetDocumentCategoryById(id.Value);
            if (documentCategory == null)
            {
                return HttpNotFound();
            }
            return View(documentCategory);
        }

        // POST: DocumentCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description")] DocumentCategory documentCategory)
        {
            if (ModelState.IsValid)
            {
                await _categories.UpdateDocumentCategory(documentCategory);
                return RedirectToAction("Index");
            }
            return View(documentCategory);
        }

        // GET: DocumentCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentCategory documentCategory = await _categories.GetDocumentCategoryById(id.Value);
            if (documentCategory == null)
            {
                return HttpNotFound();
            }
            return View(documentCategory);
        }

        // POST: DocumentCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DocumentCategory documentCategory = await _categories.GetDocumentCategoryById(id);
            await _categories.DeleteDocumentCategory(documentCategory);
            return RedirectToAction("Index");
        }
    }
}
