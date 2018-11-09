using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class SurgeryTypeController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();

        private ISurgeryType _surgeryTypes;

        public SurgeryTypeController()
        {
            this._surgeryTypes = new SurgeryTypeRepo();
        }

        // GET: /SurgeryType/
        public ActionResult Index()
        {
            return View(_surgeryTypes.GetSurgeryTypes());
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
                await _surgeryTypes.CreateSurgeryType(surgerytype);

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
            SurgeryType surgerytype = await _surgeryTypes.GetSurgeryTypeById(id.Value);
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
                await _surgeryTypes.UpdateSurgeryType(surgerytype);
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
            SurgeryType surgerytype = await _surgeryTypes.GetSurgeryTypeById(id.Value);
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
            SurgeryType surgerytype = await _surgeryTypes.GetSurgeryTypeById(id);
            await _surgeryTypes.DeleteSurgeryType(surgerytype);
            return RedirectToAction("Index");
        }
    }
}
