using AnimalDB.Models;
using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class EthicsNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EthicsNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /EthicsNumber/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.EthicsNumbers.Get());
        }

        // GET: /EthicsNumber/Archived
        public ActionResult Archived()
        {
            return View(_unitOfWork.EthicsNumbers.GetArchived());
        }

        // GET: /EthicsNumber/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description");
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName");
            return View();
        }

        // POST: /EthicsNumber/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StartYear,Text,Species_Id,Investigator_Id")] EthicsNumber ethicsnumber, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.EthicsNumbers.Insert(ethicsnumber);
                await _unitOfWork.Complete();
                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return this.Redirect(returnUrl);
                    }
                return RedirectToAction("Index");
            }

            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description", ethicsnumber.Species_Id);
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName", ethicsnumber.Investigator_Id);

            return View(ethicsnumber);
        }

        // GET: /EthicsNumber/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthicsNumber ethicsnumber = await _unitOfWork.EthicsNumbers.GetById(id.Value);
            if (ethicsnumber == null)
            {
                return HttpNotFound();
            }
            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description", ethicsnumber.Species_Id);
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName", ethicsnumber.Investigator_Id);

            return View(ethicsnumber);
        }

        // POST: /EthicsNumber/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StartYear,Text,Species_Id,Investigator_Id")] EthicsNumber ethicsnumber)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.EthicsNumbers.Update(ethicsnumber);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            ViewBag.Species_Id = new SelectList(await _unitOfWork.Species.Get(), "Id", "Description", ethicsnumber.Species_Id);
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName", ethicsnumber.Investigator_Id);
            return View(ethicsnumber);
        }

        // GET: /EthicsNumber/AddTo/5
        public async Task<ActionResult> AddTo(int? id, string animalId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Animal animal = await _unitOfWork.Animals.GetById(Convert.ToInt32(animalId));
            EthicsNumber ethics = await _unitOfWork.EthicsNumbers.GetById(id.Value);
            if (ethics == null || (!string.IsNullOrEmpty(animalId) && animal == null))
            {
                return HttpNotFound();
            }

            var model = new AddToEthicsViewModel()
            {
                EthicsId = ethics.Id,
                AnimalId = animalId
            };
            var animals = _unitOfWork.Animals.GetLiving();
            ViewBag.EthicsNumberHistoryList = _unitOfWork.EthicsNumbers.GetHistoryByEthicsId(ethics.Id);
            ViewBag.AnimalId = new SelectList(animals, "Id", "UniqueAnimalId");

            return View(model);
        }

        // POST: /EthicsNumber/AddTo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTo([Bind(Include = "AnimalId,EthicsId")] AddToEthicsViewModel ethics)
        {
            if (ModelState.IsValid)
            {
                int animalId = Convert.ToInt32(ethics.AnimalId);
                EthicsNumberHistory existingEthics = _unitOfWork.Animals.GetAnimalsEthicsNumber(animalId);
                if (existingEthics?.Ethics_Id != ethics.EthicsId)
                {
                    await _unitOfWork.Animals.AddAnimalToEthicsNumber(animalId, ethics.EthicsId);
                    await _unitOfWork.Complete();
                    return RedirectToAction("AddTo", new { Id = ethics.EthicsId });
                }
                else
                {
                    Animal animal = await _unitOfWork.Animals.GetById(animalId);
                    ModelState.AddModelError("AnimalId", "Animal \"" + animal.UniqueAnimalId + "\" is already assigned to this ethics number");
                }
            }
            var animals = _unitOfWork.Animals.GetLiving();
            ViewBag.EthicsNumberHistoryList = _unitOfWork.EthicsNumbers.GetByEthicsId(ethics.EthicsId);
            ViewBag.AnimalId = new SelectList(animals, "Id", "UniqueAnimalId");

            return View(ethics);
        }

        // GET: /Group/RemoveFrom/5
        //public async Task<ActionResult> RemoveFrom(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var animal = await db.Animals.FindAsync(id);
        //    var ethics = animal.EthicsNumbers.Last();
        //    animal.EthicsNumbers.Remove(ethics);
        //    db.Entry(animal).State = EntityState.Modified;
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("AddTo", new { Id = ethics.Id });
        //}

        // GET: /EthicsNumber/Delete/5

        public async Task<ActionResult> Archive(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthicsNumber ethicsnumber = await _unitOfWork.EthicsNumbers.GetById(id.Value);
            if (ethicsnumber == null)
            {
                return HttpNotFound();
            }
            return View(ethicsnumber);
        }

        // POST: /EthicsNumber/Archive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Archive([Bind(Include = "Id,StartYear,Text,Species_Id,Investigator_Id")] EthicsNumber ethicsnumber)
        {
            if (ModelState.IsValid)
            {
                EthicsNumber ethics = await _unitOfWork.EthicsNumbers.GetById(ethicsnumber.Id);
                _unitOfWork.EthicsNumbers.Archive(ethics);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(ethicsnumber);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthicsNumber ethicsnumber = await _unitOfWork.EthicsNumbers.GetById(id.Value);
            if (ethicsnumber == null)
            {
                return HttpNotFound();
            }
            return View(ethicsnumber);
        }

        // POST: /EthicsNumber/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EthicsNumber ethicsnumber = await _unitOfWork.EthicsNumbers.GetById(id);
            _unitOfWork.EthicsNumbers.Delete(ethicsnumber);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
