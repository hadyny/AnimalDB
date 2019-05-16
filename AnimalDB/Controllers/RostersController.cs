using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
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
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IRosterService _rosters;
        private readonly IRoomService _rooms;
        private readonly IInvestigatorService _investigators;

        public RostersController(IRosterService rosters,
                                 IRoomService rooms,
                                 IInvestigatorService investigators)
        {
            this._rosters = rosters;
            this._rooms = rooms;
            this._investigators = investigators;
        }

        // GET: RosterRooms
        public async Task<ActionResult> RosterRooms()
        {
            return View(await _rooms.GetRooms());
        }

        // GET: Rosters
        public async Task<ActionResult> Index(int? id)
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
            ViewBag.Room = room.Description;
            ViewBag.Room_Id = room.Id;
            return View(await _rosters.GetRostersByRoomId(room.Id));
        }

        // GET: Rosters/Create
        public async Task<ActionResult> Create(int? id)
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

            var model = new Roster()
            {
                Room_Id = room.Id
            };

            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _investigators.GetInvestigators())
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

            if (await _rosters.CheckIfThereIsARosterThisWeekend(roster.Date))
            {
                ModelState.AddModelError("Date", "There is already a roster for this weekend");
            }

            if (ModelState.IsValid)
            {
                await _rosters.CreateRoster(roster);
                return RedirectToAction("Index", new { id = roster.Room_Id });
            }
            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _investigators.GetInvestigators())
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
            Roster roster = await _rosters.GetRosterById(id.Value);
            if (roster == null)
            {
                return HttpNotFound();
            }

            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _investigators.GetInvestigators())
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

            if (await _rosters.CheckIfThereIsARosterThisWeekend(roster.Date, roster.Id))
            {
                ModelState.AddModelError("Date", "There is already a roster for this weekend");
            }

            if (ModelState.IsValid)
            {
                await _rosters.UpdateRoster(roster);
                return RedirectToAction("Index", new { id = roster.Room_Id });
            }
            ICollection<Student> students = new List<Student>();

            foreach (var investigator in await _investigators.GetInvestigators())
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
            Roster roster = await _rosters.GetRosterById(id.Value);
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
            Roster roster = await _rosters.GetRosterById(id);
            int room_Id = roster.Room_Id;
            await _rosters.DeleteRoster(roster);
            return RedirectToAction("Index", new { id = room_Id });
        }
    }
}