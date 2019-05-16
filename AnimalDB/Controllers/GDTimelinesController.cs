using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class GDTimelinesController : Controller
    {
        private readonly IGDTimelineService _gDTimelines;

        public GDTimelinesController(IGDTimelineService gDTimelines)
        {
            this._gDTimelines = gDTimelines;
        }

        // GET: GDTimelines
        public async Task<ActionResult> Index()
        {
            ViewBag.unused = await _gDTimelines.GetUnusedTimelineList();
            
            return View(await _gDTimelines.GetGDTimelines());
        }

        // GET: GDTimelines/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GDTimeline gDTimeline = await _gDTimelines.GetGDTimelineById(id.Value);
            if (gDTimeline == null)
            {
                return HttpNotFound();
            }
            return View(gDTimeline);
        }

        // GET: GDTimelines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GDTimelines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,Offset")] GDTimeline gDTimeline)
        {
            if (await _gDTimelines.CheckIfTimeLineExists(gDTimeline))
            {
                ModelState.AddModelError("Description", "GD Timeline already exists");
            }

            if (ModelState.IsValid)
            {
                await _gDTimelines.CreateGDTimeline(gDTimeline);
                return RedirectToAction("Index");
            }

            return View(gDTimeline);
        }

        // GET: GDTimelines/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GDTimeline gDTimeline = await _gDTimelines.GetGDTimelineById(id.Value);
            if (gDTimeline == null)
            {
                return HttpNotFound();
            }
            return View(gDTimeline);
        }

        // POST: GDTimelines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Offset")] GDTimeline gDTimeline)
        {
            if (await _gDTimelines.CheckIfTimeLineExists(gDTimeline))
            {
                ModelState.AddModelError("Description", "GD Timeline already exists");
            }
            if (ModelState.IsValid)
            {
                await _gDTimelines.UpdateGDTimeline(gDTimeline);
                return RedirectToAction("Index");
            }
            return View(gDTimeline);
        }

        // GET: GDTimelines/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GDTimeline gDTimeline = await _gDTimelines.GetGDTimelineById(id.Value);
            if (gDTimeline == null)
            {
                return HttpNotFound();
            }
            return View(gDTimeline);
        }

        // POST: GDTimelines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GDTimeline gDTimeline = await _gDTimelines.GetGDTimelineById(id);
            await _gDTimelines.DeleteGDTimeline(gDTimeline);
            return RedirectToAction("Index");
        }
    }
}
