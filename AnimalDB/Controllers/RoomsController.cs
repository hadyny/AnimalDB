using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator,Technician")]
    public class RoomsController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IRoom _rooms;
        private ITechnician _technicians;

        public RoomsController()
        {
            this._rooms = new RoomRepo();
            this._technicians = new TechnicianRepo();
        }

        // GET: Rooms
        public ActionResult Index()
        {
            return View(_rooms.GetRooms());
        }

        // GET: Rooms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _rooms.GetRoomById(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.Technician_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,Technician_Id,NoDBAnimals,EmailUpdates")] Room room)
        {
            if (ModelState.IsValid)
            {
                await _rooms.CreateRoom(room);
                return RedirectToAction("Index");
            }

            ViewBag.Technician_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName", room.Technician_Id);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _rooms.GetRoomById(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.Technician_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName", room.Technician_Id);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Technician_Id,NoDBAnimals,EmailUpdates")] Room room)
        {
            if (ModelState.IsValid)
            {
                await _rooms.UpdateRoom(room);
                return RedirectToAction("Index");
            }
            ViewBag.Technician_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName", room.Technician_Id);
            return View(room);
        }

        // GET: Rooms/Animals/5
        public async Task<ActionResult> Animals(int? id, string grps, string gmo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _rooms.GetRoomById(id.Value);
            var animals = room.Animals.Where(m => m.DeathDate == null);
            if (room == null)
            {
                return HttpNotFound();
            }
            if (!string.IsNullOrEmpty(grps))
            {
                ViewBag.BackToGroups = "1";
            }
            if (!string.IsNullOrEmpty(gmo))
            {
                animals = animals.Where(m => m.ApprovalNumber_Id != null);
            }


            ViewBag.RoomId = id.Value;
            ViewBag.RoomName = room.Description;
            return View(animals);
        }

        // GET: Rooms/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _rooms.GetRoomById(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Room room = await _rooms.GetRoomById(id);
            await _rooms.DeleteRoom(room);
            return RedirectToAction("Index");
        }
    }
}
