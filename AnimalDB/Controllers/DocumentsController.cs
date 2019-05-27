using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Web.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private IDocumentService _docs;
        private IDocumentCategoryService _categories;

        public DocumentsController(IDocumentService docs, IDocumentCategoryService categories)
        {
            this._docs = docs;
            this._categories = categories;
        }

        // GET: Documents
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentCategory category = await _categories.GetDocumentCategoryById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.Id = category.Id;
            ViewBag.Category = category.Description;

            return View(await _docs.GetDocumentsByCategoryId(category.Id));
        }


        // GET: Documents/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Category_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description");
            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,FileName,Content,DateUploaded,Category_Id")] Document document)
        {
            if (ModelState.IsValid)
            {
                await _docs.CreateDocument(document);
                return RedirectToAction("Index", "DocumentCategories", document.Category_Id);
            }

            ViewBag.Category_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await _docs.GetDocumentById(id.Value);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,FileName,Content,DateUploaded,Category_Id")] Document document, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (ModelState.IsValid)
            {
                var olddoc = await _docs.GetDocumentById(document.Id);
                if (upload != null && upload.ContentLength > 0)
                {
                    olddoc.DateUploaded = DateTime.Now;
                    olddoc.FileName = System.IO.Path.GetFileName(upload.FileName);
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        olddoc.Content = reader.ReadBytes(upload.ContentLength);
                    }
                }
                olddoc.Description = document.Description;
                olddoc.Category_Id = document.Category_Id;

                await _docs.UpdateDocument(olddoc);

                return RedirectToAction("Index", "DocumentCategories", new { id = document.Category_Id });
            }
            ViewBag.Category_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        [Authorize(Roles = "Administrator,Technician")]
        // GET: Documents/Upload
        public async Task<ActionResult> Upload(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Category_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", id.Value);
            }
            else
            {
                ViewBag.Category_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description");
            }

            ViewBag.returnUrl = Request["returnUrl"] ?? Url.Action("Index", "DocumentCategories", new { id });

            return View();
        }

        // POST: Sops/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload([Bind(Include = "Id,Description,FileName,DateUploaded,Category_Id")] Document document, HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength <= 0 || upload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (await _docs.DoesDocumentFileNameExist(upload.FileName))
            {
                ModelState.AddModelError("file", "\"" + upload.FileName + "\" already exists in the database. If you want to update the file, locate the entry and click \"Edit\".");
            }

            document.DateUploaded = DateTime.Now;
            document.FileName = System.IO.Path.GetFileName(upload.FileName);
            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
                document.Content = reader.ReadBytes(upload.ContentLength);
            }

            if (ModelState.IsValid)
            {
                await _docs.CreateDocument(document);
                return RedirectToAction("Index", "DocumentCategories", new { id = document.Category_Id });
            }
            ViewBag.returnUrl = Request["returnUrl"] ?? Url.Action("Index", "DocumentCategories", new { id = document.Category_Id });
            ViewBag.Category_Id = new SelectList(await _categories.GetDocumentCategories(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        // GET: Documents/View/5
        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document doc = await _docs.GetDocumentById(id.Value);
            if (doc == null)
            {
                return HttpNotFound();
            }

            return View(doc);
        }

        // GET: Documents/GetDocument/5
        public async Task<FileResult> GetDocument(int? id, int? download)
        {
            if (id == null)
            {
                return null;
            }
            Document doc = await _docs.GetDocumentById(id.Value);
            if (doc == null)
            {
                return null;
            }

            if (download.HasValue)
            {
                return File(doc.Content, "application/pdf", doc.FileName);
            }
            else
            {
                return File(doc.Content, "application/pdf");
            }
        }

        // GET: Documents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await _docs.GetDocumentById(id.Value);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Document document = await _docs.GetDocumentById(id);
            int catergory = document.Category_Id;
            await _docs.DeleteDocument(document);
            return RedirectToAction("Index", "DocumentCategories", new { id = catergory });
        }
    }
}
