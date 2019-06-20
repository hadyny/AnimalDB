using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;

        public AdministratorsController(IUnitOfWork unitOfWork, IUserManagementService userManagementService)
        {
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
        }

        // GET: Administrators
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Administrators.Get());
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
            string error = await _userManagementService.Register(administrator, Repo.Enums.UserType.Administrator);

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
            Administrator administrator = await _unitOfWork.Administrators.GetById(id);
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
            Administrator administrator = await _unitOfWork.Administrators.GetById(id);
            var administrators = await _unitOfWork.Administrators.Get();

            if (administrators.Count() == 1)
            {
                ModelState.AddModelError("", "Deletion of administrator failed: There must be at least one administrator");
            }
            else
            {
                await _unitOfWork.Administrators.Delete(administrator);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            
            return View(administrator);
        }
    }
}
