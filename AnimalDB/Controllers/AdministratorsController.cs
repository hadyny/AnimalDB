using AnimalDB.Functions;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorsController : Controller
    {
        private IAdministrator _administrators;

        public AdministratorsController()
        {
            this._administrators = new AdministratorRepo();
        }

        // GET: Administrators
        public ActionResult Index()
        {
            return View(_administrators.GetAdministrators());
        }
        
        // GET: Administrators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administrators/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Administrator administrator)
        {
            string error = await UserManagement.CreateAnimalUser(administrator, Repo.Enums.UserType.Administrator);

            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", error);
            }
            
            return View(administrator);
        }


        // GET: Administrators/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = await _administrators.GetAdministratorById(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

        // POST: Administrators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Administrator administrator = await _administrators.GetAdministratorById(id);

            if (_administrators.GetAdministrators().Count() == 1)
            {
                ModelState.AddModelError("", "Deletion of administrator failed: There must be at least one administrator");
            }
            else
            {
                await _administrators.DeleteAdministrator(administrator);
                return RedirectToAction("Index");
            }
            
            return View(administrator);
        }
    }
}
