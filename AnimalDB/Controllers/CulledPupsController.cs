using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AnimalDB.Repo.Services;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Models;
using AnimalDB.Repo.Contexts;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Investigator, Technician, Administrator")]
    public class CulledPupsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CulledPupsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CulledPups
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.CulledPups.Get());
        }

        // GET: CulledPups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulledPups culledPups = await _unitOfWork.CulledPups.GetById(id.Value);
            if (culledPups == null)
            {
                return HttpNotFound();
            }
            return View(culledPups);
        }

        // GET: CulledPups/Create
        public ActionResult Create()
        {
            var animals = _unitOfWork.Animals.GetLiving();
            ViewBag.AnimalId = new SelectList(animals
                                                .Where(m => m.Sex == Sex.Female), 
                                            "Id", "UniqueAnimalId");
            return View();
        }

        // POST: CulledPups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AnimalId,AmountCulled,DateCulled,NumFemale,NumMale")] CulledPups culledPups)
        {
            if (culledPups.NumMale + culledPups.NumFemale != culledPups.AmountCulled || 
                (culledPups.NumFemale == null && culledPups.NumMale == null))
            {
                ModelState.AddModelError("AmountCulled", "Male and Female totals don't add up");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.CulledPups.Insert(culledPups);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            var animals = _unitOfWork.Animals.GetLiving();
            ViewBag.AnimalId = new SelectList(animals
                                                .Where(m => m.Sex == Sex.Female),
                                           "Id", "UniqueAnimalId", culledPups.AnimalId);
            return View(culledPups);
        }

        // GET: CulledPups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulledPups culledPups = await _unitOfWork.CulledPups.GetById(id.Value);
            if (culledPups == null)
            {
                return HttpNotFound();
            }
            var animals = _unitOfWork.Animals.GetLiving();
            ViewBag.AnimalId = new SelectList(animals                                                
                                                .Where(m => m.Sex == Sex.Female),
                                           "Id", "UniqueAnimalId", culledPups.AnimalId);
            return View(culledPups);
        }

        // POST: CulledPups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AnimalId,AmountCulled,DateCulled,NumFemale,NumMale")] CulledPups culledPups)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CulledPups.Update(culledPups);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            var animals = _unitOfWork.Animals.GetLiving();
            ViewBag.AnimalId = new SelectList(animals
                                                .Where(m => m.Sex == Sex.Female),
                                           "Id", "UniqueAnimalId", culledPups.AnimalId);
            return View(culledPups);
        }

        // GET: CulledPups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulledPups culledPups = await _unitOfWork.CulledPups.GetById(id.Value);
            if (culledPups == null)
            {
                return HttpNotFound();
            }
            return View(culledPups);
        }

        // POST: CulledPups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CulledPups culledPups = await _unitOfWork.CulledPups.GetById(id);
            _unitOfWork.CulledPups.Delete(culledPups);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }


        // GET: CulledPups/PupsByEthics/5
        public async Task<ActionResult> PupsByEthics()
        {
            var model = new List<PupsByEthicsViewModel>();
            EthicsNumberHistory entry;

            foreach (var animal in await _unitOfWork.CulledPups.Get())
            {
                entry = animal.Animal.EthicsNumbers.OrderByDescending(n => n.Timestamp).FirstOrDefault();

                if (entry != null && model.Count(m => m.Ethics == entry.EthicsNumber.Text) == 0)
                {
                    model.Add(new PupsByEthicsViewModel() {
                        Ethics = entry.EthicsNumber.Text,
                        TotalFemale = animal.NumFemale.Value,
                        TotalMale = animal.NumMale.Value
                    });
                }
                else if (entry != null)
                {
                    model.First(m => m.Ethics == entry.EthicsNumber.Text).TotalFemale += animal.NumFemale.Value;
                    model.First(m => m.Ethics == entry.EthicsNumber.Text).TotalMale += animal.NumMale.Value;
                }
            }
           
            return View(model);
        }

    }
}
