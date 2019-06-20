using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Web.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Documents
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DocumentCategory category = await _unitOfWork.DocumentCategories.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.Id = category.Id;
            ViewBag.Category = category.Description;

            return View(_unitOfWork.Documents.GetByCategoryId(category.Id));
        }


        // GET: Documents/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Category_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description");
            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,FileName,Content,DateUploaded,Category_Id")] Document document)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Documents.Insert(document);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", "DocumentCategories", document.Category_Id);
            }

            ViewBag.Category_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await _unitOfWork.Documents.GetById(id.Value);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,FileName,Content,DateUploaded,Category_Id")] Document document, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentType != System.Net.Mime.MediaTypeNames.Application.Pdf)
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (ModelState.IsValid)
            {
                var olddoc = await _unitOfWork.Documents.GetById(document.Id);
                if (upload != null && upload.ContentLength > 0)
                {
                    var filename = Path.GetFileName(upload.FileName);
                    olddoc.DateUploaded = DateTime.Now;
                    olddoc.FileName = filename;
                    var path = Path.Combine(Server.MapPath("~/Content/Documents"), filename);
                    upload.SaveAs(path);
                }
                olddoc.Description = document.Description;
                olddoc.Category_Id = document.Category_Id;

                _unitOfWork.Documents.Update(olddoc);
                await _unitOfWork.Complete();

                return RedirectToAction("Index", "DocumentCategories", new { id = document.Category_Id });
            }
            ViewBag.Category_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        [Authorize(Roles = "Administrator,Technician")]
        // GET: Documents/Upload
        public async Task<ActionResult> Upload(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Category_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", id.Value);
            }
            else
            {
                ViewBag.Category_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description");
            }

            ViewBag.returnUrl = Request["returnUrl"] ?? Url.Action("Index", "DocumentCategories", new { id });

            return View();
        }

        // POST: Documents/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload([Bind(Include = "Id,Description,FileName,DateUploaded,Category_Id")] Document document, HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength <= 0 || upload.ContentType != System.Net.Mime.MediaTypeNames.Application.Pdf)
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (_unitOfWork.Documents.Exists(upload.FileName))
            {
                ModelState.AddModelError("file", "\"" + upload.FileName + "\" already exists in the database. If you want to update the file, locate the entry and click \"Edit\".");
            }
           
            var filename = Path.GetFileName(upload.FileName);
            document.DateUploaded = DateTime.Now;
            document.FileName = filename;
            var path = Path.Combine(Server.MapPath("~/Content/Documents"), filename);            

            if (ModelState.IsValid)
            {
                _unitOfWork.Documents.Insert(document);
                await _unitOfWork.Complete();
                upload.SaveAs(path);
                return RedirectToAction("Index", "DocumentCategories", new { id = document.Category_Id });
            }
            ViewBag.returnUrl = Request["returnUrl"] ?? Url.Action("Index", "DocumentCategories", new { id = document.Category_Id });
            ViewBag.Category_Id = new SelectList(await _unitOfWork.DocumentCategories.Get(), "Id", "Description", document.Category_Id);
            return View(document);
        }

        // GET: Documents/View/5
        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document doc = await _unitOfWork.Documents.GetById(id.Value);
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
            Document doc = await _unitOfWork.Documents.GetById(id.Value);
            var content = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/Content/Documents"), doc.FileName));
            if (doc == null)
            {
                return null;
            }

            if (download.HasValue)
            {
                return File(content, System.Net.Mime.MediaTypeNames.Application.Pdf, doc.FileName);
            }
            else
            {
                return File(content, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
        }

        // GET: Documents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await _unitOfWork.Documents.GetById(id.Value);
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
            Document document = await _unitOfWork.Documents.GetById(id);
            int catergory = document.Category_Id;
            _unitOfWork.Documents.Delete(document);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", "DocumentCategories", new { id = catergory });
        }
    }
}
