using AnimalDB.Repo.Contexts;
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
        private readonly IUnitOfWork _unitOfWork;

        public DocumentCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: DocumentCategories
        public async Task<ActionResult> Index(int? id)
        {
            var category = id.HasValue ? await _unitOfWork.DocumentCategories.GetById(id.Value) : null;

            var model = new DocumentCategoryViewModel() {
                CategoryName = category?.Description,
                Category_Id = category?.Id,
                Parent_Id = id,
                IsRootCategory = !id.HasValue,
                AuthenticatedUser = User.IsInRole("Administrator") || User.IsInRole("Technician"),
                Documents = _unitOfWork.Documents.GetByCategoryId(id),
                SubCategories = _unitOfWork.DocumentCategories.GetByParentId(id),
                ParentHierarchy = _unitOfWork.DocumentCategories.GetParentHierarchy(category)
            };

            return View(model);
        }

        // GET: DocumentCategories/Create
        public async Task<ActionResult> Create(int? id)
        {
            ViewBag.Id = id;
            ViewBag.ParentCategory_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", id);
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
                _unitOfWork.DocumentCategories.Insert(documentCategory);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id });
            }
            ViewBag.Id = id;
            ViewBag.ParentCategory_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", documentCategory.ParentCategory_Id);
            return View(documentCategory);
        }

        // GET: DocumentCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentCategory documentCategory = await _unitOfWork.DocumentCategories.GetById(id.Value);
            if (documentCategory == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentCategory_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", documentCategory.ParentCategory_Id);
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
                _unitOfWork.DocumentCategories.Update(documentCategory);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id = documentCategory.Id });
            }

            ViewBag.ParentCategory_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", documentCategory.ParentCategory_Id);
            return View(documentCategory);
        }

        // GET: DocumentCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentCategory documentCategory = await _unitOfWork.DocumentCategories.GetById(id.Value);
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
            DocumentCategory documentCategory = await _unitOfWork.DocumentCategories.GetById(id);
             _unitOfWork.DocumentCategories.Delete(documentCategory);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
