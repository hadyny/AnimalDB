using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class ArrivalStatusController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArrivalStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /ArrivalStatus/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.ArrivalStatus.Get());
        }

        // GET: /ArrivalStatus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArrivalStatus arrivalstatus = await _unitOfWork.ArrivalStatus.GetById(id.Value);
            if (arrivalstatus == null)
            {
                return HttpNotFound();
            }
            return View(arrivalstatus);
        }

        // GET: /ArrivalStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ArrivalStatus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description,Type")] ArrivalStatus arrivalstatus)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ArrivalStatus.Insert(arrivalstatus);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(arrivalstatus);
        }

        // GET: /ArrivalStatus/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArrivalStatus arrivalstatus = await _unitOfWork.ArrivalStatus.GetById(id.Value);
            if (arrivalstatus == null)
            {
                return HttpNotFound();
            }
            return View(arrivalstatus);
        }

        // POST: /ArrivalStatus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description,Type")] ArrivalStatus arrivalstatus)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ArrivalStatus.Update(arrivalstatus);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(arrivalstatus);
        }

        // GET: /ArrivalStatus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArrivalStatus arrivalstatus = await _unitOfWork.ArrivalStatus.GetById(id.Value);
            if (arrivalstatus == null)
            {
                return HttpNotFound();
            }
            return View(arrivalstatus);
        }

        // POST: /ArrivalStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ArrivalStatus arrivalstatus = await _unitOfWork.ArrivalStatus.GetById(id);
            _unitOfWork.ArrivalStatus.Delete(arrivalstatus);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
