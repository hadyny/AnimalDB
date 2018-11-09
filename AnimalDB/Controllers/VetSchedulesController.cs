using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class VetSchedulesController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IVetSchedule _vetSchedules;

        public VetSchedulesController()
        {
            this._vetSchedules = new VetScheduleRepo();
        }

        // GET: VetSchedules
        [Authorize]
        public ActionResult Index(string returnUrl)
        {
            if (_vetSchedules.GetVetSchedules().Count() != 0)
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
            var doc = _vetSchedules.GetVetSchedules().FirstOrDefault();

            if (doc == null)
            {
                return null;
            }

            return File(doc.Content, "application/pdf");
            
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
            if (upload == null || upload.ContentLength <= 0 || upload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("file", "A pdf file is required");
            }

            vetSchedule.DateUploaded = DateTime.Now;

            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
                vetSchedule.Content = reader.ReadBytes(upload.ContentLength);
            }

            if (ModelState.IsValid)
            {
                await _vetSchedules.CreateVetSchedule(vetSchedule);
                return RedirectToAction("Index");
            }
            
            return View(vetSchedule);
        }
        [Authorize(Roles = "Technician,Administrator")]
        // GET: VetSchedules/Delete/5
        public ActionResult Delete()
        {
            VetSchedule vetSchedule = _vetSchedules.GetVetSchedules().FirstOrDefault();
            
            return View(vetSchedule);
        }

        // POST: VetSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed()
        {
            VetSchedule vetSchedule = _vetSchedules.GetVetSchedules().FirstOrDefault();
            if (vetSchedule != null)
            {
                await _vetSchedules.DeleteVetSchedule(vetSchedule);
            }
            return RedirectToAction("Index");
        }
    }
}
