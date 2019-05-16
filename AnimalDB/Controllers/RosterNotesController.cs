using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class RosterNotesController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IRosterNoteService _rosterNotes;
        private IRosterService _rosters;

        public RosterNotesController(IRosterNoteService rosterNotes, IRosterService rosters)
        {
            this._rosterNotes = rosterNotes;
            this._rosters = rosters;
        }

        // GET: RosterNotes
        public async Task<ActionResult> Index(int? id)
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

            if (roster.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                ViewBag.Weekend = roster.Date.ToLongDateString() + " - " + roster.Date.AddDays(1).ToLongDateString();
            }
            else
            {
                ViewBag.Weekend = roster.Date.AddDays(-1).ToLongDateString() + " - " + roster.Date.ToLongDateString();
            }

            ViewBag.Id = roster.Id;
            ViewBag.Room_Id = roster.Room_Id;
            return View(await _rosterNotes.GetRosterNotesByRosterId(id.Value));
        }

        // GET: RosterNotes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterNote rosterNote = await _rosterNotes.GetRosterNoteById(id.Value);
            if (rosterNote == null)
            {
                return HttpNotFound();
            }
            return View(rosterNote);
        }

        // GET: RosterNotes/Create
        public async Task<ActionResult> Create(int? id)
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

            ViewBag.RosterId = roster.Id;
            if (roster.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                ViewBag.Weekend = roster.Date.ToLongDateString() + " - " + roster.Date.AddDays(1).ToLongDateString();
            }
            else
            {
                ViewBag.Weekend = roster.Date.AddDays(-1).ToLongDateString() + " - " + roster.Date.ToLongDateString();
            }

            return View();
        }

        // POST: RosterNotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Content")] RosterNote rosterNote, int id)
        {
            if (ModelState.IsValid)
            {
                rosterNote.Roster_Id = id;
                await _rosterNotes.CreateRosterNote(rosterNote);
                return RedirectToAction("Index", new { id = id });
            }
            Roster roster = await _rosters.GetRosterById(id);
            if (roster == null)
            {
                return HttpNotFound();
            }
            if (roster.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                ViewBag.Weekend = roster.Date.ToLongDateString() + " - " + roster.Date.AddDays(1).ToLongDateString();
            }
            else
            {
                ViewBag.Weekend = roster.Date.AddDays(-1).ToLongDateString() + " - " + roster.Date.ToLongDateString();
            }
            ViewBag.RosterId = id;

            return View(rosterNote);
        }

        // GET: RosterNotes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterNote rosterNote = await _rosterNotes.GetRosterNoteById(id.Value);
            if (rosterNote == null)
            {
                return HttpNotFound();
            }
            return View(rosterNote);
        }

        // POST: RosterNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Content,Roster_Id")] RosterNote rosterNote)
        {
            if (ModelState.IsValid)
            {
                await _rosterNotes.UpdateRosterNote(rosterNote);
                return RedirectToAction("Index", new { id = rosterNote.Roster_Id });
            }
            return View(rosterNote);
        }

        // GET: RosterNotes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterNote rosterNote = await _rosterNotes.GetRosterNoteById(id.Value);
            if (rosterNote == null)
            {
                return HttpNotFound();
            }
            return View(rosterNote);
        }

        // POST: RosterNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RosterNote rosterNote = await _rosterNotes.GetRosterNoteById(id);
            int rosterId = rosterNote.Roster_Id;
            await _rosterNotes.DeleteRosterNote(rosterNote);
            return RedirectToAction("Index", new { id = rosterId });
        }
    }
}
