using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TechnicianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;

        public TechnicianController(IUnitOfWork unitOfWork, IUserManagementService userManagementService)
        {
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
        }

        // GET: /Technician/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Technicians.Get());
        }

        // GET: /Technician/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="UserName,Email,FirstName,LastName")] Technician technician)
        {
            string error = await _userManagementService.Register(technician, Repo.Enums.UserType.Technician);

            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", error);
            }

            return View(technician);
        }

        // GET: /Technician/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technician Technician = await _unitOfWork.Technicians.GetById(id);
            if (Technician == null)
            {
                return HttpNotFound();
            }
            return View(Technician);
        }

        // POST: /Technician/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Technician technician = await _unitOfWork.Technicians.GetById(id);
            await _unitOfWork.Technicians.Delete(technician);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
