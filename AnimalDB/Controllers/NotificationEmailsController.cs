using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Investigator, Technician, Administrator")]
    public class NotificationEmailsController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();

        private INotificationEmail _notificationEmails;
        private IInvestigator _investigators;

        public NotificationEmailsController()
        {
            this._notificationEmails = new NotificationEmailRepo();
            this._investigators = new InvestigatorRepo();
        }

        // GET: NotificationEmails
        public ActionResult Index()
        {
            return View(_notificationEmails.GetNotificationEmails());
        }

        // GET: NotificationEmails/Create
        public ActionResult Create()
        {
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName");
            return View();
        }

        // POST: NotificationEmails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Email")] NotificationEmail notificationEmail)
        {
            if (User.IsInRole("Investigator"))
            {
                var investigator = _investigators.GetInvestigatorByUsername(User.Identity.Name);
                notificationEmail.Investigator_Id = investigator.Id;
            }

            if (ModelState.IsValid)
            {
                await _notificationEmails.CreateNotificationEmail(notificationEmail);
                return RedirectToAction("Index");
            }
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName");
            return View(notificationEmail);
        }

        // GET: NotificationEmails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationEmail notificationEmail = await _notificationEmails.GetNotificationEmailById(id.Value);
            if (notificationEmail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName", notificationEmail.Investigator_Id);
            return View(notificationEmail);
        }

        // POST: NotificationEmails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,Investigator_Id")] NotificationEmail notificationEmail)
        {
            if (User.IsInRole("Investigator"))
            {
                var investigator = _investigators.GetInvestigatorByUsername(User.Identity.Name);
                notificationEmail.Investigator_Id = investigator.Id;
            }
            if (ModelState.IsValid)
            {
                await _notificationEmails.UpdateNotificationEmail(notificationEmail);
                return RedirectToAction("Index");
            }
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName", notificationEmail.Investigator_Id);
            return View(notificationEmail);
        }

        // GET: NotificationEmails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationEmail notificationEmail = await _notificationEmails.GetNotificationEmailById(id.Value);
            if (notificationEmail == null)
            {
                return HttpNotFound();
            }
            return View(notificationEmail);
        }

        // POST: NotificationEmails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NotificationEmail notificationEmail = await _notificationEmails.GetNotificationEmailById(id);
            await _notificationEmails.DeleteNotificationEmail(notificationEmail);
            return RedirectToAction("Index");
        }
    }
}
