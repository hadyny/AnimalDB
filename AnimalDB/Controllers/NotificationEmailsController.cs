using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Investigator, Technician, Administrator")]
    public class NotificationEmailsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationEmailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: NotificationEmails
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.NotificationEmails.Get());
        }

        // GET: NotificationEmails/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName");
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
                var investigator = await _unitOfWork.Investigators.GetByUsername(User.Identity.Name);
                notificationEmail.Investigator_Id = investigator.Id;
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.NotificationEmails.Insert(notificationEmail);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName");
            return View(notificationEmail);
        }

        // GET: NotificationEmails/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationEmail notificationEmail = await _unitOfWork.NotificationEmails.GetById(id.Value);
            if (notificationEmail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName", notificationEmail.Investigator_Id);
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
                var investigator = await _unitOfWork.Investigators.GetByUsername(User.Identity.Name);
                notificationEmail.Investigator_Id = investigator.Id;
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.NotificationEmails.Update(notificationEmail);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName", notificationEmail.Investigator_Id);
            return View(notificationEmail);
        }

        // GET: NotificationEmails/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotificationEmail notificationEmail = await _unitOfWork.NotificationEmails.GetById(id.Value);
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
            NotificationEmail notificationEmail = await _unitOfWork.NotificationEmails.GetById(id);
            _unitOfWork.NotificationEmails.Delete(notificationEmail);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
