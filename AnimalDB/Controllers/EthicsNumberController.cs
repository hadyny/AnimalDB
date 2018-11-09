using AnimalDB.Models;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class EthicsNumberController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IEthicsNumber _ethicsNumbers;
        private ISpecies _species;
        private IInvestigator _investigators;
        private IAnimal _animals;
        private IEthicsNumberHistory _ethicsNumberHistories;

        public EthicsNumberController()
        {
            this._ethicsNumbers = new EthicsNumberRepo();
            this._ethicsNumberHistories = new EthicsNumberHistoryRepo();
            this._species = new SpeciesRepo();
            this._investigators = new InvestigatorRepo();
            this._animals = new AnimalRepo();
        }

        // GET: /EthicsNumber/
        public ActionResult Index()
        {
            return View(_ethicsNumbers.GetEthicsNumbers());
        }

        // GET: /EthicsNumber/Archived
        public ActionResult Archived()
        {
            return View(_ethicsNumbers.GetArchivedNumbers());
        }

        // GET: /EthicsNumber/Create
        public ActionResult Create(string returnUrl)
        {
            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description");
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName");
            return View();
        }

        // POST: /EthicsNumber/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StartYear,Text,Species_Id,Investigator_Id")] EthicsNumber ethicsnumber, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                await _ethicsNumbers.CreateEthicsNumber(ethicsnumber);
                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return this.Redirect(returnUrl);
                    }
                return RedirectToAction("Index");
            }

            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description", ethicsnumber.Species_Id);
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName", ethicsnumber.Investigator_Id);

            return View(ethicsnumber);
        }

        // GET: /EthicsNumber/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EthicsNumber ethicsnumber = await _ethicsNumbers.GetEthicsNumberById(id.Value);
            if (ethicsnumber == null)
            {
                return HttpNotFound();
            }
            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description", ethicsnumber.Species_Id);
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName", ethicsnumber.Investigator_Id);

            return View(ethicsnumber);
        }

        // POST: /EthicsNumber/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StartYear,Text,Species_Id,Investigator_Id")] EthicsNumber ethicsnumber)
        {
            if (ModelState.IsValid)
            {
                await _ethicsNumbers.UpdateEthicsNumber(ethicsnumber);
                return RedirectToAction("Index");
            }
            ViewBag.Species_Id = new SelectList(_species.GetSpecies(), "Id", "Description", ethicsnumber.Species_Id);
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName", ethicsnumber.Investigator_Id);
            return View(ethicsnumber);
        }

        // GET: /EthicsNumber/AddTo/5
        public async Task<ActionResult> AddTo(int? id, string animalId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Animal animal = await _animals.GetAnimalById(Convert.ToInt32(animalId));
            EthicsNumber ethics = await _ethicsNumbers.GetEthicsNumberById(id.Value);
            if (ethics == null || (!string.IsNullOrEmpty(animalId) && animalId == null))
            {
                return HttpNotFound();
            }

            var model = new AddToEthicsViewModel()
            {
                EthicsId = ethics.Id,
                AnimalId = animalId
            };

            ViewBag.EthicsNumberHistoryList = _ethicsNumbers.GetEthicsNumberHistoryByEthicsId(ethics.Id).ToList();
            ViewBag.AnimalId = new SelectList(_animals.GetLivingAnimals(), "Id", "UniqueAnimalId");

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
                var existingEthics = await _animals.GetAnimalsEthicsNumber(animalId);
                if (existingEthics?.Ethics_Id != ethics.EthicsId)
                {
                    await _animals.AddAnimalToEthicsNumber(animalId, ethics.EthicsId);
                    return RedirectToAction("AddTo", new { Id = ethics.EthicsId });
                }
                else
                {
                    ModelState.AddModelError("AnimalId", "Animal \"" + _animals.GetAnimalById(animalId).Result.UniqueAnimalId + "\" is already assigned to this ethics number");
                }
            }

            ViewBag.EthicsNumberHistoryList = _ethicsNumbers.GetEthicsNumberHistoryByEthicsId(ethics.EthicsId);
            ViewBag.AnimalId = new SelectList(_animals.GetLivingAnimals(), "Id", "UniqueAnimalId");

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
            EthicsNumber ethicsnumber = await _ethicsNumbers.GetEthicsNumberById(id.Value);
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
                EthicsNumber ethics = await _ethicsNumbers.GetEthicsNumberById(ethicsnumber.Id);
                await _ethicsNumbers.ArchiveEthics(ethics);
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
            EthicsNumber ethicsnumber = await _ethicsNumbers.GetEthicsNumberById(id.Value);
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
            EthicsNumber ethicsnumber = await _ethicsNumbers.GetEthicsNumberById(id);
            await _ethicsNumbers.DeleteEthicsNumber(ethicsnumber);
            return RedirectToAction("Index");
        }
    }
}
