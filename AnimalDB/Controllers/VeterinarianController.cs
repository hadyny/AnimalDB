using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VeterinarianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;

        public VeterinarianController(IUnitOfWork unitOfWork, IUserManagementService userManagementService)
        {
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
        }

        // GET: /Veterinarian/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Veterinarians.Get());
        }

        // GET: /Veterinarian/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="UserName,Email,FirstName,LastName")] Veterinarian veterinarian)
        {
            string error = await _userManagementService.Register(veterinarian, Repo.Enums.UserType.Veterinarian);

            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", error);
            }

            return View(veterinarian);
        }

        // GET: /Veterinarian/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinarian Veterinarian = await _unitOfWork.Veterinarians.GetById(id);
            if (Veterinarian == null)
            {
                return HttpNotFound();
            }
            return View(Veterinarian);
        }

        // POST: /Veterinarian/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Veterinarian veterinarian = await _unitOfWork.Veterinarians.GetById(id);
            await _unitOfWork.Veterinarians.Delete(veterinarian);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
