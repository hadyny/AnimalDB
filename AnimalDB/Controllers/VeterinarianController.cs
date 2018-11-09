using AnimalDB.Functions;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VeterinarianController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IVeterinarian _veterinarians;

        public VeterinarianController()
        {
            this._veterinarians = new VeterinarianRepo();
        }


        // GET: /Veterinarian/
        public ActionResult Index()
        {
            return View(_veterinarians.GetVeterinarians());
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
            string error = await UserManagement.CreateAnimalUser(veterinarian, Repo.Enums.UserType.Veterinarian);

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
            Veterinarian Veterinarian = await _veterinarians.GetVeterinarianById(id);
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
            Veterinarian veterinarian = await _veterinarians.GetVeterinarianById(id);
            await _veterinarians.DeleteVeterinarian(veterinarian);
            return RedirectToAction("Index");
        }
    }
}
