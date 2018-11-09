using AnimalDB.Functions;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
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
        private IInvestigator _investigators;

        public InvestigatorController()
        {
            this._investigators = new InvestigatorRepo();
        }

        // GET: /Investigator/
        public ActionResult Index()
        {
            return View(_investigators.GetInvestigators());
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
            string error = await UserManagement.CreateAnimalUser(investigator, Repo.Enums.UserType.Investigator);

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
