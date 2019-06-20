using AnimalDB.Repo.Contexts;
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
        private readonly IUnitOfWork _unitOfWork;

        public GDTimelinesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: GDTimelines
        public async Task<ActionResult> Index()
        {
            ViewBag.unused = _unitOfWork.GDTimelines.GetUnused();
            return View(await _unitOfWork.GDTimelines.Get());
        }

        // GET: GDTimelines/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GDTimeline gDTimeline = await _unitOfWork.GDTimelines.GetById(id.Value);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,Offset")] GDTimeline gDTimeline)
        {
            if (_unitOfWork.GDTimelines.Exists(gDTimeline))
            {
                ModelState.AddModelError("Description", "GD Timeline already exists");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.GDTimelines.Insert(gDTimeline);
                await _unitOfWork.Complete();
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
            GDTimeline gDTimeline = await _unitOfWork.GDTimelines.GetById(id.Value);
            if (gDTimeline == null)
            {
                return HttpNotFound();
            }
            return View(gDTimeline);
        }

        // POST: GDTimelines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,Offset")] GDTimeline gDTimeline)
        {
            if (_unitOfWork.GDTimelines.Exists(gDTimeline))
            {
                ModelState.AddModelError("Description", "GD Timeline already exists");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.GDTimelines.Update(gDTimeline);
                await _unitOfWork.Complete();
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
            GDTimeline gDTimeline = await _unitOfWork.GDTimelines.GetById(id.Value);
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
            GDTimeline gDTimeline = await _unitOfWork.GDTimelines.GetById(id);
            _unitOfWork.GDTimelines.Delete(gDTimeline);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
