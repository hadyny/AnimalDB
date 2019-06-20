using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class VetSchedulesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VetSchedulesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: VetSchedules
        [Authorize]
        public ActionResult Index(string returnUrl)
        {
            if (_unitOfWork.VetSchedules.Exists())
            {
                ViewBag.Exists = true;
            }
            else
            { 
                ViewBag.Exists = false;
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.returnUrl = returnUrl;
            }else
            {
                ViewBag.returnUrl = Url.Action("RosterRooms", "Rosters");
            }

            return View();
        }

        [Authorize]
        // GET: VetSchedules/GetDoc/5
        public FileResult GetDoc()
        {
            var doc = _unitOfWork.VetSchedules.GetVetSchedule();
            var content = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/VetSchedule/Schedule.pdf"));

            if (doc == null)
            {
                return null;
            }

            return File(content, System.Net.Mime.MediaTypeNames.Application.Pdf);
            
        }

        [Authorize(Roles = "Technician,Administrator")]
        // GET: VetSchedules/Upload
        public ActionResult Upload()
        {
            return View();
        }
        
        // POST: VetSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload([Bind(Include = "Id,Content")] VetSchedule vetSchedule, HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength <= 0 || upload.ContentType != System.Net.Mime.MediaTypeNames.Application.Pdf)
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            vetSchedule.DateUploaded = DateTime.Now;
            var path = Server.MapPath("~/Content/VetSchedule/Schedule.pdf");

            if (ModelState.IsValid)
            {
                _unitOfWork.VetSchedules.Insert(vetSchedule);
                upload.SaveAs(path);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            
            return View(vetSchedule);
        }
        [Authorize(Roles = "Technician,Administrator")]
        // GET: VetSchedules/Delete/5
        public ActionResult Delete()
        {
            VetSchedule vetSchedule = _unitOfWork.VetSchedules.GetVetSchedule();
            
            return View(vetSchedule);
        }

        // POST: VetSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed()
        {
            VetSchedule vetSchedule = _unitOfWork.VetSchedules.GetVetSchedule();
            if (vetSchedule != null)
            {
                _unitOfWork.VetSchedules.Delete(vetSchedule);
                await _unitOfWork.Complete();
            }
            return RedirectToAction("Index");
        }
    }
}
