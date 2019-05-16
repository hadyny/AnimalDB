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
        //private AnimalDBContext db = new AnimalDBContext();

        private INotificationService _notifications;

        public NotificationsController(INotificationService notifications)
        {
            this._notifications = notifications;
        }

        // GET: Notifications
        public async Task<ActionResult> Index()
        {
            IEnumerable<Notification> notifications;

            if (User.IsInRole("Investigator"))
            {
                notifications = await _notifications.GetNotificationByInvestigatorUsername(User.Identity.Name);
            }
            else
            {
                notifications = await _notifications.GetNotifications();
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
            foreach (var notification in await _notifications.GetPastNotifications())
            {
                await _notifications.DeleteNotification(notification);
            }
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
            Notification notification = await _notifications.GetNotificationById(id.Value);
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
            Notification notification = await _notifications.GetNotificationById(id);
            await _notifications.DeleteNotification(notification);
            return RedirectToAction("Index");
        }
    }
}
