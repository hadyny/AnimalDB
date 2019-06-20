using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator,Technician")]
    public class EthicsDocumentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EthicsDocumentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: EthicsDocuments/Investigators

        public async Task<ActionResult> Investigators()
        {
            return View(await _unitOfWork.Investigators.Get());
        }

        // GET: EthicsDocuments
        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Investigator inv = await _unitOfWork.Investigators.GetById(id);

            if (inv == null)
            {
                return HttpNotFound();
            }
            ViewBag.Investigator = inv.FullName;
            ViewBag.Investigator_Id = inv.Id;

            return View(_unitOfWork.EthicsDocuments.GetByInvestigatorId(id));
        }

        // GET: EthicsDocuments/Upload
        public async Task<ActionResult> Upload(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Investigator inv = await _unitOfWork.Investigators.GetById(id);

            if (inv == null)
            {
                return HttpNotFound();
            }

            var model = new EthicsDocument()
            {
                Investigator_Id = id
            };

            ViewBag.Investigator_Id = id;
            ViewBag.Investigator = inv.FullName;
            return View(model);
        }

        // POST: EthicsDocuments/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload([Bind(Include = "Id,Description,FileName,Content,DateUploaded,Investigator_Id")] EthicsDocument ethicsDocument, HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength <= 0 || upload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (_unitOfWork.EthicsDocuments.Exists(upload.FileName))
            {
                ModelState.AddModelError("file", "\"" + upload.FileName + "\" already exists in the database. If you want to update the file, locate the entry and click \"Edit\".");
            }

            ethicsDocument.FileName = System.IO.Path.GetFileName(upload.FileName);
            ethicsDocument.DateUploaded = DateTime.Now;

            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
                ethicsDocument.Content = reader.ReadBytes(upload.ContentLength);
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.EthicsDocuments.Insert(ethicsDocument);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id = ethicsDocument.Investigator_Id });
            }
            Investigator inv = await _unitOfWork.Investigators.GetById(ethicsDocument.Investigator_Id);

            ViewBag.Investigator_Id = inv.Id;
            ViewBag.Investigator = inv.FullName;

            return View(ethicsDocument);
        }

        // GET: EthicsDocuments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthicsDocument ethicsDocument = await _unitOfWork.EthicsDocuments.GetById(id.Value);
            if (ethicsDocument == null)
            {
                return HttpNotFound();
            }
            
            return View(ethicsDocument);
        }

        // POST: EthicsDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,FileName,Content,DateUploaded,Investigator_Id")] EthicsDocument ethicsDocument, HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            if (ModelState.IsValid)
            {
                var olddoc = await _unitOfWork.EthicsDocuments.GetById(ethicsDocument.Id);
                if (upload != null && upload.ContentLength > 0)
                {
                    olddoc.DateUploaded = DateTime.Now;
                    olddoc.FileName = System.IO.Path.GetFileName(upload.FileName);
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        olddoc.Content = reader.ReadBytes(upload.ContentLength);
                    }
                }
                olddoc.Description = ethicsDocument.Description;

                _unitOfWork.EthicsDocuments.Update(olddoc);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id = ethicsDocument.Investigator_Id });
            }
            
            return View(ethicsDocument);
        }

        // GET: EthicsDocuments/GetDoc/5
        public async Task<FileResult> GetDoc(int? id, int? download)
        {
            if (id == null)
            {
                return null;
            }
            EthicsDocument doc = await _unitOfWork.EthicsDocuments.GetById(id.Value);
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

        // GET: EthicsDocument/View/5
        public async Task<ActionResult> View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthicsDocument doc = await _unitOfWork.EthicsDocuments.GetById(id.Value);
            if (doc == null)
            {
                return HttpNotFound();
            }

            return View(doc);
        }

        // GET: EthicsDocuments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthicsDocument ethicsDocument = await _unitOfWork.EthicsDocuments.GetById(id.Value);
            if (ethicsDocument == null)
            {
                return HttpNotFound();
            }
            return View(ethicsDocument);
        }

        // POST: EthicsDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EthicsDocument ethicsDocument = await _unitOfWork.EthicsDocuments.GetById(id);
            string inv_id = ethicsDocument.Investigator_Id;
            _unitOfWork.EthicsDocuments.Delete(ethicsDocument);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id = inv_id });
        }

    }
}
