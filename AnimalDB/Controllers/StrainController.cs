using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class StrainController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StrainController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Strain/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Strains.Get());
        }


        // GET: /Strain/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description");
            return View();
        }

        // POST: /Strain/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description,Species_Id,Code")] Strain strain)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Strains.Insert(strain);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description", strain.Species_Id);
            return View(strain);
        }

        // GET: /Strain/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Strain strain = await _unitOfWork.Strains.GetById(id.Value);
            if (strain == null)
            {
                return HttpNotFound();
            }
            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description", strain.Species_Id);

            return View(strain);
        }

        // POST: /Strain/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description,Species_Id,Code")] Strain strain)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Strains.Update(strain);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description", strain.Species_Id);
            return View(strain);
        }

        // GET: /Strain/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Strain strain = await _unitOfWork.Strains.GetById(id.Value);
            if (strain == null)
            {
                return HttpNotFound();
            }
            return View(strain);
        }

        // POST: /Strain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Strain strain = await _unitOfWork.Strains.GetById(id);
            _unitOfWork.Strains.Delete(strain);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
