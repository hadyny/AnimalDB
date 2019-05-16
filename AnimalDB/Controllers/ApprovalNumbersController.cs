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
        private IApprovalNumberService _approvalNumbers;

        public ApprovalNumbersController(IApprovalNumberService approvalNumbers)
        {
            this._approvalNumbers = approvalNumbers;
        }

        // GET: ApprovalNumbers
        public async Task<ActionResult> Index()
        {
            return View(await _approvalNumbers.GetApprovalNumbers());
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
                await _approvalNumbers.CreateApprovalNumber(approvalNumber);
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
            ApprovalNumber approvalNumber = await _approvalNumbers.GetApprovalNumberById(id.Value);
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
                await _approvalNumbers.UpdateApprovalNumber(approvalNumber);
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
            ApprovalNumber approvalNumber = await _approvalNumbers.GetApprovalNumberById(id.Value);
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
            ApprovalNumber approvalNumber = await _approvalNumbers.GetApprovalNumberById(id);
            await _approvalNumbers.DeleteApprovalNumber(approvalNumber);
            return RedirectToAction("Index");
        }

    }
}
