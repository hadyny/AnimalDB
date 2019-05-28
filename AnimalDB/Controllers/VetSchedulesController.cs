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
        //private AnimalDBContext db = new AnimalDBContext();
        private IVetScheduleService _vetSchedules;

        public VetSchedulesController(IVetScheduleService vetSchedules)
        {
            this._vetSchedules = vetSchedules;
        }

        // GET: VetSchedules
        [Authorize]
        public async Task<ActionResult> Index(string returnUrl)
        {
            if (await _vetSchedules.DoesVetScheduleExist())
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
        public async Task<FileResult> GetDoc()
        {
            var doc = await _vetSchedules.GetVetSchedule();
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
                await _vetSchedules.CreateVetSchedule(vetSchedule);
                upload.SaveAs(path);
                return RedirectToAction("Index");
            }
            
            return View(vetSchedule);
        }
        [Authorize(Roles = "Technician,Administrator")]
        // GET: VetSchedules/Delete/5
        public async Task<ActionResult> Delete()
        {
            VetSchedule vetSchedule = await _vetSchedules.GetVetSchedule();
            
            return View(vetSchedule);
        }

        // POST: VetSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed()
        {
            VetSchedule vetSchedule = await _vetSchedules.GetVetSchedule();
            if (vetSchedule != null)
            {
                await _vetSchedules.DeleteVetSchedule(vetSchedule);
            }
            return RedirectToAction("Index");
        }
    }
}
