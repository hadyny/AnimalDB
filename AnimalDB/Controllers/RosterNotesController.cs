using AnimalDB.Repo.Contexts;
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
        private readonly IUnitOfWork _unitOfWork;

        public RosterNotesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: RosterNotes
        public async Task<ActionResult> Index(int? id)
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
            return View(_unitOfWork.RosterNotes.GetByRosterId(id.Value));
        }

        // GET: RosterNotes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RosterNote rosterNote = await _unitOfWork.RosterNotes.GetById(id.Value);
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
            Roster roster = await _unitOfWork.Rosters.GetById(id.Value);
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
                _unitOfWork.RosterNotes.Insert(rosterNote);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id = id });
            }
            Roster roster = await _unitOfWork.Rosters.GetById(id);
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
            RosterNote rosterNote = await _unitOfWork.RosterNotes.GetById(id.Value);
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
                _unitOfWork.RosterNotes.Update(rosterNote);
                await _unitOfWork.Complete();
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
            RosterNote rosterNote = await _unitOfWork.RosterNotes.GetById(id.Value);
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
            RosterNote rosterNote = await _unitOfWork.RosterNotes.GetById(id);
            int rosterId = rosterNote.Roster_Id;
            _unitOfWork.RosterNotes.Delete(rosterNote);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id = rosterId });
        }
    }
}
