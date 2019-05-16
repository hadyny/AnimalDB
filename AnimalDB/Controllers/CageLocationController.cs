using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class CageLocationController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IRoomService _rooms;
        private readonly ICageLocationService _cageLocations;

        public CageLocationController(IRoomService rooms, ICageLocationService cageLocations)
        {
            this._rooms = rooms;
            this._cageLocations = cageLocations;
        }

        // GET: /CageLocation/
        public async Task<ActionResult> Index()
        {
            return View(await _cageLocations.GetCageLocations());
        }

        // GET: /CageLocation/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description");
            return View();
        }

        // POST: /CageLocation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Room_Id,Description")] CageLocation cagelocation, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                await _cageLocations.CreateCageLocation(cagelocation);
                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return this.Redirect(returnUrl);
                    }
                return RedirectToAction("Index");
            }
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description");
            return View(cagelocation);
        }

        // GET: /CageLocation/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CageLocation cagelocation = await _cageLocations.GetCageLocationById(id.Value);
            if (cagelocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", cagelocation.Room_Id);
            return View(cagelocation);
        }

        // POST: /CageLocation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Room_Id")] CageLocation cagelocation)
        {
            if (ModelState.IsValid)
            {
                await _cageLocations.UpdateCageLocation(cagelocation);
                return RedirectToAction("Index");
            }
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", cagelocation.Room_Id);
            return View(cagelocation);
        }

        // GET: /CageLocation/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CageLocation cagelocation = await _cageLocations.GetCageLocationById(id.Value);
            if (cagelocation == null)
            {
                return HttpNotFound();
            }
            return View(cagelocation);
        }

        // POST: /CageLocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CageLocation cagelocation = await _cageLocations.GetCageLocationById(id);
            await _cageLocations.DeleteCageLocation(cagelocation);
            return RedirectToAction("Index");
        }
    }
}
