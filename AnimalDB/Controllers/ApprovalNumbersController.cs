using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class ApprovalNumbersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalNumbersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: ApprovalNumbers
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.ApprovalNumbers.Get());
        }

        // GET: ApprovalNumbers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApprovalNumbers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,FriendlyName")] ApprovalNumber approvalNumber)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ApprovalNumbers.Insert(approvalNumber);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(approvalNumber);
        }

        // GET: ApprovalNumbers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovalNumber approvalNumber = await _unitOfWork.ApprovalNumbers.GetById(id.Value);
            if (approvalNumber == null)
            {
                return HttpNotFound();
            }
            return View(approvalNumber);
        }

        // POST: ApprovalNumbers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description,FriendlyName")] ApprovalNumber approvalNumber)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ApprovalNumbers.Update(approvalNumber);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(approvalNumber);
        }

        // GET: ApprovalNumbers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApprovalNumber approvalNumber = await _unitOfWork.ApprovalNumbers.GetById(id.Value);
            if (approvalNumber == null)
            {
                return HttpNotFound();
            }
            return View(approvalNumber);
        }

        // POST: ApprovalNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ApprovalNumber approvalNumber = await _unitOfWork.ApprovalNumbers.GetById(id);
            _unitOfWork.ApprovalNumbers.Delete(approvalNumber);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

    }
}
