using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator,Technician")]
    public class InvestigatorController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IInvestigatorService _investigators;
        private readonly IUserManagementService _users;

        public InvestigatorController(IInvestigatorService investigators, IUserManagementService users)
        {
            this._investigators = investigators;
            this._users = users;
        }

        // GET: /Investigator/
        public async Task<ActionResult> Index()
        {
            return View(await _investigators.GetInvestigators());
        }

        // GET: /Investigator/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="UserName,Email,FirstName,LastName")] Investigator investigator)
        {
            string error = await _users.CreateAnimalUser(investigator, Repo.Enums.UserType.Investigator);

            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", error);
            }

            return View(investigator);
        }

        // GET: /Investigator/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Investigator investigator = await _investigators.GetInvestigatorById(id);
            if (investigator == null)
            {
                return HttpNotFound();
            }
            return View(investigator);
        }

        // POST: /Investigator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var investigator = await _investigators.GetInvestigatorById(id);
            await _investigators.DeleteInvestigator(investigator);
            return RedirectToAction("Index");
        }
    }
}
