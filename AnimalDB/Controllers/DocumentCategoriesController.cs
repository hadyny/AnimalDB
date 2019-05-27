using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Web.Models;
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
        private readonly IDocumentService _documents;

        public DocumentCategoriesController(IDocumentCategoryService categories, IDocumentService documents)
        {
            this._categories = categories;
            this._documents = documents;
        }

        // GET: DocumentCategories
        public async Task<ActionResult> Index(int? id)
        {
            var category = id.HasValue ? await _categories.GetDocumentCategoryById(id.Value) : null;

            var model = new DocumentCategoryViewModel() {
                CategoryName = category?.Description,
                Category_Id = category?.Id,
                Parent_Id = id,
                IsRootCategory = !id.HasValue,
                AuthenticatedUser = User.IsInRole("Administrator") || User.IsInRole("Technician"),
                Documents = await _documents.GetDocumentsByCategoryId(id), // takes ages
                SubCategories = await _categories.GetDocumentCategoriesByParentId(id),
                ParentHierarchy = _categories.GetParentHierarchy(category)
            };

            return View(model);
        }

        // GET: DocumentCategories/Create
        public async Task<ActionResult> Create(int? id)
        {
            ViewBag.Id = id;
            ViewBag.ParentCategory_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", id);
            return View();
        }

        // POST: DocumentCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,ParentCategory_Id")] DocumentCategory documentCategory, int? id)
        {
            if (ModelState.IsValid)
            {
                await _categories.CreateDocumentCategory(documentCategory);
                return RedirectToAction("Index", new { id });
            }
            ViewBag.Id = id;
            ViewBag.ParentCategory_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", documentCategory.ParentCategory_Id);
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

            ViewBag.ParentCategory_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", documentCategory.ParentCategory_Id);
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
                return RedirectToAction("Index", new { id = documentCategory.Id });
            }

            ViewBag.ParentCategory_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", documentCategory.ParentCategory_Id);
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
