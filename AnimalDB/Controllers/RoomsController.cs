using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Services;
using AnimalDB.Repo.Interfaces;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AnimalDB.Repo.Contexts;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator,Technician")]
    public class RoomsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Rooms
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Rooms.Get());
        }

        // GET: Rooms/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _unitOfWork.Rooms.GetById(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Technician_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,Technician_Id,NoDBAnimals,EmailUpdates")] Room room)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Rooms.Insert(room);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            ViewBag.Technician_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName", room.Technician_Id);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _unitOfWork.Rooms.GetById(id.Value);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.Technician_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName", room.Technician_Id);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Technician_Id,NoDBAnimals,EmailUpdates")] Room room)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Rooms.Update(room);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            ViewBag.Technician_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName", room.Technician_Id);
            return View(room);
        }

        // GET: Rooms/Animals/5
        public async Task<ActionResult> Animals(int? id, string grps, string gmo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = await _unitOfWork.Rooms.GetById(id.Value);
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
            Room room = await _unitOfWork.Rooms.GetById(id.Value);
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
            Room room = await _unitOfWork.Rooms.GetById(id);
            _unitOfWork.Rooms.Delete(room);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
