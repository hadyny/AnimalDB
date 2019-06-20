using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Investigator, Technician, Administrator")]
    public class NotificationsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Notifications
        public async Task<ActionResult> Index()
        {
            IEnumerable<Notification> notifications;

            if (User.IsInRole("Investigator"))
            {
                notifications = _unitOfWork.Notifications.GetByInvestigatorUsername(User.Identity.Name);
            }
            else
            {
                notifications = await _unitOfWork.Notifications.Get();
            }

            return View(notifications);
        }

        public ActionResult DismissAll()
        {
            return View();
        }

        [HttpPost, ActionName("DismissAll")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DismissAllConfirmed()
        {
            foreach (var notification in _unitOfWork.Notifications.GetPast())
            {
                _unitOfWork.Notifications.Delete(notification);
            }
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }


        // GET: Notifications/Delete/5
        public async Task<ActionResult> Delete(int? id, string task)
        {
            if (task == "dismiss")
            {
                ViewBag.Action = "Dismiss";
            }
            else
            {
                ViewBag.Action = "Delete";
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = await _unitOfWork.Notifications.GetById(id.Value);
            if (notification == null)
            {
                return HttpNotFound();
            }
            
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Notification notification = await _unitOfWork.Notifications.GetById(id);
            _unitOfWork.Notifications.Delete(notification);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
