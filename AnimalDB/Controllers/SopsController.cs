using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class SopsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SopsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Sops
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SopCategory category = await _unitOfWork.SopCategories.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Id = category.Id;
            ViewBag.Category = category.Description;

            return View(_unitOfWork.Sops.GetByCategoryId(category.Id));
        }
        [Authorize(Roles = "Administrator,Technician")]
        // GET: Sops/Upload
        public async Task<ActionResult> Upload(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Category_Id = new SelectList(await _unitOfWork.SopCategories.Get(), "Id", "Description", id.Value);
            }
            else
            {
                ViewBag.Category_Id = new SelectList(await _unitOfWork.SopCategories.Get(), "Id", "Description");
            }

            ViewBag.returnUrl = Request["returnUrl"] ?? Url.Action("Index", "SopCategories");

            return View();
        }

        // POST: Sops/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload([Bind(Include = "Id,Description,FileName,DateUploaded,Category_Id")] Sop sop, HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength <= 0 || upload.ContentType != System.Net.Mime.MediaTypeNames.Application.Pdf)
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }
            
            if (_unitOfWork.Sops.FileNameExists(upload.FileName))
            {
                ModelState.AddModelError("file", "\"" + upload.FileName + "\" already exists in the database. If you want to update the file, locate the entry and click \"Edit\".");
            }

            var filename = Path.GetFileName(upload.FileName);
            sop.DateUploaded = DateTime.Now;
            sop.FileName = filename;
            var path = Path.Combine(Server.MapPath("~/Content/Sops"), filename);

            if (ModelState.IsValid)
            {
                _unitOfWork.Sops.Insert(sop);
                await _unitOfWork.Complete();
                upload.SaveAs(path);
                return RedirectToAction("Index", new { id = sop.Category_Id });
            }
            ViewBag.returnUrl = Request["returnUrl"] ?? Url.Action("Index", "SopCategories");
            ViewBag.Category_Id = new SelectList(await _unitOfWork.SopCategories.Get(), "Id", "Description", sop.Category_Id);
            return View(sop);
        }

        // GET: Sops/GetSop/5
        public async Task<FileResult> GetSop(int? id, int? download)
        {
            if (id == null)
            {
                return null;
            }
            Sop sop = await _unitOfWork.Sops.GetById(id.Value);
            var content = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/Content/Sops"), sop.FileName));
            if (sop == null)
            {
                return null;
            }

            if (download.HasValue)
            {
                return File(content, System.Net.Mime.MediaTypeNames.Application.Pdf, sop.FileName);
            }
            else
            {
                return File(content, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
        }

        // GET: Sops/View/5
        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sop sop = await _unitOfWork.Sops.GetById(id.Value);
            if (sop == null)
            {
                return HttpNotFound();
            }
            
            return View(sop);
        }

        [Authorize(Roles = "Administrator,Technician")]
        // GET: Sops/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sop sop = await _unitOfWork.Sops.GetById(id.Value);
            if (sop == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(await _unitOfWork.SopCategories.Get(), "Id", "Description", sop.Category_Id);
            return View(sop);
        }

        // POST: Sops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,FileName,DateUploaded,Category_Id")] Sop sop, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentType != System.Net.Mime.MediaTypeNames.Application.Pdf)
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (ModelState.IsValid)
            {
                var oldsop = await _unitOfWork.Sops.GetById(sop.Id);
                if (upload != null && upload.ContentLength > 0)
                {
                    var filename = Path.GetFileName(upload.FileName);
                    oldsop.DateUploaded = DateTime.Now;
                    oldsop.FileName = filename;
                    var path = Path.Combine(Server.MapPath("~/Content/Sops"), filename);
                    upload.SaveAs(path);
                }
                oldsop.Description = sop.Description;
                oldsop.Category_Id = sop.Category_Id;

                _unitOfWork.Sops.Update(oldsop);
                await _unitOfWork.Complete();

                return RedirectToAction("Index", new { id = sop.Category_Id });
            }
            ViewBag.Category_Id = new SelectList(await _unitOfWork.SopCategories.Get(), "Id", "Description", sop.Category_Id);
            return View(sop);
        }

        [Authorize(Roles = "Administrator,Technician")]
        // GET: Sops/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sop sop = await _unitOfWork.Sops.GetById(id.Value);
            if (sop == null)
            {
                return HttpNotFound();
            }
            return View(sop);
        }

        // POST: Sops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sop sop = await _unitOfWork.Sops.GetById(id);
            int cat_id = sop.Category_Id;
            _unitOfWork.Sops.Delete(sop);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id = cat_id });
        }
    }
}
