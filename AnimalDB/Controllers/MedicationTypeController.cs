using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class MedicationTypeController : Controller
    {
        private IMedicationTypeService _medicationTypes;

        public MedicationTypeController(IMedicationTypeService medicationTypes)
        {
            this._medicationTypes = medicationTypes;
        }

        // GET: /MedicationType/
        public async Task<ActionResult> Index()
        {
            return View(await _medicationTypes.GetMedicationTypes());
        }

        // GET: /MedicationType/Create
        public ActionResult Create(string returnUrl)
        {
            return View();
        }

        // POST: /MedicationType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description")] MedicationType medicationtype, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                await _medicationTypes.CreateMedicationType(medicationtype);
                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }

            return View(medicationtype);
        }

        // GET: /MedicationType/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicationType medicationtype = await _medicationTypes.GetMedicationTypeById(id.Value);
            if (medicationtype == null)
            {
                return HttpNotFound();
            }
            return View(medicationtype);
        }

        // POST: /MedicationType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description")] MedicationType medicationtype)
        {
            if (ModelState.IsValid)
            {
                await _medicationTypes.UpdateMedicationType(medicationtype);
                return RedirectToAction("Index");
            }
            return View(medicationtype);
        }

        //GET: /MedicationType/Animals/5
        public async Task<ActionResult> Animals(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicationType medicationtype = await _medicationTypes.GetMedicationTypeById(id.Value);
            if (medicationtype == null)
            {
                return HttpNotFound();
            }

            return View(medicationtype);
        }



        // GET: /MedicationType/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicationType medicationtype = await _medicationTypes.GetMedicationTypeById(id.Value);
            if (medicationtype == null)
            {
                return HttpNotFound();
            }
            return View(medicationtype);
        }

        // POST: /MedicationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MedicationType medicationtype = await _medicationTypes.GetMedicationTypeById(id);
            await _medicationTypes.DeleteMedicationType(medicationtype);
            return RedirectToAction("Index");
        }
    }
}
