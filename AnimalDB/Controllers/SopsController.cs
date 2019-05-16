using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class SopsController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly ISopService _sops;
        private readonly ISopCategoryService _sopCategories;

        public SopsController(ISopService sops, ISopCategoryService sopCategories)
        {
            this._sops = sops;
            this._sopCategories = sopCategories;
        }

        // GET: Sops
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SopCategory category = await _sopCategories.GetSopCategoryById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Id = category.Id;
            ViewBag.Category = category.Description;

            return View(await _sops.GetSopsByCategoryId(category.Id));
        }
        [Authorize(Roles = "Administrator,Technician")]
        // GET: Sops/Upload
        public async Task<ActionResult> Upload(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.Category_Id = new SelectList(await _sopCategories.GetSopCategories(), "Id", "Description", id.Value);
            }
            else
            {
                ViewBag.Category_Id = new SelectList(await _sopCategories.GetSopCategories(), "Id", "Description");
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
            if (upload == null || upload.ContentLength <= 0 || upload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }
            
            if (await _sops.DoesSopFileNameExist(upload.FileName))
            {
                ModelState.AddModelError("file", "\"" + upload.FileName + "\" already exists in the database. If you want to update the file, locate the entry and click \"Edit\".");
            }

            sop.DateUploaded = DateTime.Now;
            sop.FileName = System.IO.Path.GetFileName(upload.FileName);
            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
                sop.Content = reader.ReadBytes(upload.ContentLength);
            }

            if (ModelState.IsValid)
            {
                await _sops.CreateSop(sop);
                return RedirectToAction("Index", new { id = sop.Category_Id });
            }
            ViewBag.returnUrl = Request["returnUrl"] ?? Url.Action("Index", "SopCategories");
            ViewBag.Category_Id = new SelectList(await _sopCategories.GetSopCategories(), "Id", "Description", sop.Category_Id);
            return View(sop);
        }

        // GET: Sops/GetSop/5
        public async Task<FileResult> GetSop(int? id, int? download)
        {
            if (id == null)
            {
                return null;
            }
            Sop sop = await _sops.GetSopById(id.Value);
            if (sop == null)
            {
                return null;
            }

            if (download.HasValue)
            {
                return File(sop.Content, "application/pdf", sop.FileName);
            }
            else
            {
                return File(sop.Content, "application/pdf");
            }
        }

        // GET: Sops/View/5
        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sop sop = await _sops.GetSopById(id.Value);
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
            Sop sop = await _sops.GetSopById(id.Value);
            if (sop == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(await _sopCategories.GetSopCategories(), "Id", "Description", sop.Category_Id);
            return View(sop);
        }

        // POST: Sops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,FileName,DateUploaded,Category_Id")] Sop sop, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (ModelState.IsValid)
            {
                var oldsop = await _sops.GetSopById(sop.Id);
                if (upload != null && upload.ContentLength > 0)
                {
                    oldsop.DateUploaded = DateTime.Now;
                    oldsop.FileName = System.IO.Path.GetFileName(upload.FileName);
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        oldsop.Content = reader.ReadBytes(upload.ContentLength);
                    }
                }
                oldsop.Description = sop.Description;
                oldsop.Category_Id = sop.Category_Id;

                await _sops.UpdateSop(oldsop);

                return RedirectToAction("Index", new { id = sop.Category_Id });
            }
            ViewBag.Category_Id = new SelectList(await _sopCategories.GetSopCategories(), "Id", "Description", sop.Category_Id);
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
            Sop sop = await _sops.GetSopById(id.Value);
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
            Sop sop = await _sops.GetSopById(id);
            int cat_id = sop.Category_Id;
            await _sops.DeleteSop(sop);
            return RedirectToAction("Index", new { id = cat_id });
        }
    }
}
