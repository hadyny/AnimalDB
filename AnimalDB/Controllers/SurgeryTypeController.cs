using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class SurgeryTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SurgeryTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /SurgeryType/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.SurgeryTypes.Get());
        }

        // GET: /SurgeryType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /SurgeryType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Description")] SurgeryType surgerytype, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SurgeryTypes.Insert(surgerytype);
                await _unitOfWork.Complete();

                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(returnUrl);
                }

                return RedirectToAction("Index");
            
            }
            return View(surgerytype);
        }

        // GET: /SurgeryType/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurgeryType surgerytype = await _unitOfWork.SurgeryTypes.GetById(id.Value);
            if (surgerytype == null)
            {
                return HttpNotFound();
            }
            return View(surgerytype);
        }

        // POST: /SurgeryType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description")] SurgeryType surgerytype)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SurgeryTypes.Update(surgerytype);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(surgerytype);
        }

        // GET: /SurgeryType/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurgeryType surgerytype = await _unitOfWork.SurgeryTypes.GetById(id.Value);
            if (surgerytype == null)
            {
                return HttpNotFound();
            }
            return View(surgerytype);
        }

        // POST: /SurgeryType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SurgeryType surgerytype = await _unitOfWork.SurgeryTypes.GetById(id);
            _unitOfWork.SurgeryTypes.Delete(surgerytype);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
