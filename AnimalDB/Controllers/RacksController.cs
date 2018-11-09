using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace AnimalDB.Controllers
{
    [Authorize]
    public class RacksController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();

        private IRack _racks;
        private IRoom _rooms;

        public RacksController()
        {
            this._racks = new RackRepo();
            this._rooms = new RoomRepo();
        }

        // GET: Racks
        public ActionResult Index()
        {
            return View(_racks.GetRacks());
        }

        // GET: Racks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rack rack = await _racks.GetRackById(id.Value);
            if (rack == null)
            {
                return HttpNotFound();
            }
            return View(rack);
        }

        // GET: Racks/AnimalGrid/5
        public async Task<ActionResult> AnimalGrid(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rack rack = await _racks.GetRackById(id.Value);
            if (rack == null)
            {
                return HttpNotFound();
            }
            return View(rack);
        }

        // GET: Racks/Create
        public ActionResult Create()
        {
            ViewBag.Room_Id = new SelectList(_rooms.GetRooms(), "Id", "Description");
            return View();
        }

        // POST: Racks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Reference_Id,Room_Id,Width,Height")] Rack rack)
        {
            if (ModelState.IsValid)
            {
                await _racks.CreateRack(rack);
                return RedirectToAction("Index");
            }

            ViewBag.Room_Id = new SelectList(_rooms.GetRooms(), "Id", "Description", rack.Room_Id);
            return View(rack);
        }

        // GET: Racks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rack rack = await _racks.GetRackById(id.Value);
            if (rack == null)
            {
                return HttpNotFound();
            }
            ViewBag.Room_Id = new SelectList(_rooms.GetRooms(), "Id", "Description", rack.Room_Id);
            return View(rack);
        }

        // POST: Racks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Reference_Id,Room_Id,Width,Height")] Rack rack)
        {
            if (ModelState.IsValid)
            {
                await _racks.UpdateRack(rack);
                return RedirectToAction("Index");
            }
            ViewBag.Room_Id = new SelectList(_rooms.GetRooms(), "Id", "Description", rack.Room_Id);
            return View(rack);
        }

        // GET: Racks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rack rack = await _racks.GetRackById(id.Value);
            if (rack == null)
            {
                return HttpNotFound();
            }
            return View(rack);
        }

        // POST: Racks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rack rack = await _racks.GetRackById(id);
            await _racks.DeleteRack(rack);
            return RedirectToAction("Index");
        }
    }
}
