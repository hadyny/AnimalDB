using AnimalDB.Repo.Contexts;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;

        public InvestigatorController(IUnitOfWork unitOfWork, IUserManagementService userManagementService)
        {
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
        }

        // GET: /Investigator/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Investigators.Get());
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
            string error = await _userManagementService.Register(investigator, Repo.Enums.UserType.Investigator);

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
            Investigator investigator = await _unitOfWork.Investigators.GetById(id);
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
            var investigator = await _unitOfWork.Investigators.GetById(id);
            await _unitOfWork.Investigators.Delete(investigator);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
