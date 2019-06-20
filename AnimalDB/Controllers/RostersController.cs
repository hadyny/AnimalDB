using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class RostersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RostersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: RosterRooms
        public async Task<ActionResult> RosterRooms()
        {
            return View(await _unitOfWork.Rooms.Get());
        }

        // GET: Rosters
        public async Task<ActionResult> Index(int? id)
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
            ViewBag.Room = room.Description;
            ViewBag.Room_Id = room.Id;
            return View(_unitOfWork.Rosters.GetByRoomId(room.Id));
        }

        // GET: Rosters/Create
        public async Task<ActionResult> Create(int? id)
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

            var model = new Roster()
            {
                Room_Id = room.Id
            };

            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _unitOfWork.Investigators.Get())
            {
                if (investigator.Animals.Count(m => m.Room_Id == room.Id) != 0)
                {
                    students = students.Concat(investigator.Students).ToList();
                }
            }

            ViewBag.Student_Id = new SelectList(students, "Id", "FirstName");
            return View(model);
        }

        // POST: Rosters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Student_Id,Date,Room_Id")] Roster roster)
        {
            if (roster.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                roster.Date = roster.Date.AddDays(-1);
            }

            if (_unitOfWork.Rosters.ThisWeekendsRosterExists(roster.Date))
            {
                ModelState.AddModelError("Date", "There is already a roster for this weekend");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Rosters.Insert(roster);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id = roster.Room_Id });
            }
            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _unitOfWork.Investigators.Get())
            {
                if (investigator.Animals.Count(m => m.Room_Id == roster.Room_Id) != 0)
                {
                    students = students.Concat(investigator.Students).ToList();
                }
            }
            ViewBag.Student_Id = new SelectList(students, "Id", "FirstName", roster.Student_Id);
            return View(roster);
        }

        // GET: Rosters/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roster roster = await _unitOfWork.Rosters.GetById(id.Value);
            if (roster == null)
            {
                return HttpNotFound();
            }

            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _unitOfWork.Investigators.Get())
            {
                if (investigator.Animals.Count(m => m.Room_Id == roster.Room_Id) != 0)
                {
                    students = students.Concat(investigator.Students).ToList();
                }
            }
            ViewBag.Student_Id = new SelectList(students, "Id", "FirstName", roster.Student_Id);
            return View(roster);
        }

        // POST: Rosters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Student_Id,Date,Room_Id")] Roster roster)
        {
            if (roster.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                roster.Date = roster.Date.AddDays(-1);
            }

            if (_unitOfWork.Rosters.ThisWeekendsRosterExists(roster.Date, roster.Id))
            {
                ModelState.AddModelError("Date", "There is already a roster for this weekend");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Rosters.Update(roster);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id = roster.Room_Id });
            }
            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _unitOfWork.Investigators.Get())
            {
                if (investigator.Animals.Count(m => m.Room_Id == roster.Room_Id) != 0)
                {
                    students = students.Concat(investigator.Students).ToList();
                }
            }
            ViewBag.Student_Id = new SelectList(students, "Id", "FirstName", roster.Student_Id);
            return View(roster);
        }

        // GET: Rosters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roster roster = await _unitOfWork.Rosters.GetById(id.Value);
            if (roster == null)
            {
                return HttpNotFound();
            }
            return View(roster);
        }

        // POST: Rosters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Roster roster = await _unitOfWork.Rosters.GetById(id);
            int room_Id = roster.Room_Id;
            _unitOfWork.Rosters.Delete(roster);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id = room_Id });
        }
    }
}