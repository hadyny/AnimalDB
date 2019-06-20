using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace AnimalDB.Controllers
{
    [Authorize]
    public class RacksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RacksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Racks
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Racks.Get());
        }

        // GET: Racks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rack rack = await _unitOfWork.Racks.GetById(id.Value);
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
            Rack rack = await _unitOfWork.Racks.GetById(id.Value);
            if (rack == null)
            {
                return HttpNotFound();
            }
            return View(rack);
        }

        // GET: Racks/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Room_Id = new SelectList(await _unitOfWork.Rooms.Get(), "Id", "Description");
            return View();
        }

        // POST: Racks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Reference_Id,Room_Id,Width,Height")] Rack rack)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Racks.Insert(rack);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            ViewBag.Room_Id = new SelectList(await _unitOfWork.Rooms.Get(), "Id", "Description", rack.Room_Id);
            return View(rack);
        }

        // GET: Racks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rack rack = await _unitOfWork.Racks.GetById(id.Value);
            if (rack == null)
            {
                return HttpNotFound();
            }
            ViewBag.Room_Id = new SelectList(await _unitOfWork.Rooms.Get(), "Id", "Description", rack.Room_Id);
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
                _unitOfWork.Racks.Update(rack);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            ViewBag.Room_Id = new SelectList(await _unitOfWork.Rooms.Get(), "Id", "Description", rack.Room_Id);
            return View(rack);
        }

        // GET: Racks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rack rack = await _unitOfWork.Racks.GetById(id.Value);
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
            Rack rack = await _unitOfWork.Racks.GetById(id);
            _unitOfWork.Racks.Delete(rack);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
