using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class EthicsNumberHistoryController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IAnimalService _animals;
        private readonly IEthicsNumberHistoryService _ethicsNumberHistories;
        private readonly IEthicsNumberService _ethicsNumbers;

        public EthicsNumberHistoryController(IAnimalService animals, 
                                             IEthicsNumberHistoryService ethicsNumberHistories,
                                             IEthicsNumberService ethicsNumbers)
        {
            this._animals = animals;
            this._ethicsNumberHistories = ethicsNumberHistories;
            this._ethicsNumbers = ethicsNumbers;
        }


        // GET: /EthicsnumberHistory/
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;

            return View(await _ethicsNumberHistories.GetEthicsNumberHistoriesByAnimal(id.Value));
        }

        // GET: /EthicsnumberHistory/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text");
            var model = new EthicsNumberHistory
            {
                Timestamp = DateTime.Now
            };
            return View(model);
        }

        // POST: /EthicsnumberHistory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EthicsNumberHistory ethicsnumberhistory, int? id)
        {
            ethicsnumberhistory.Timestamp = DateTime.Now;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }

            ethicsnumberhistory.Animal_Id = id.Value;

            if (ModelState.IsValid)
            {
                await _animals.AddAnimalToEthicsNumber(animal.Id, ethicsnumberhistory.Ethics_Id);
                return RedirectToAction("Index", new { id });
            }

            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text", ethicsnumberhistory.Ethics_Id);
            return View(ethicsnumberhistory);
        }

        // GET: /EthicsnumberHistory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ethicsnumberhistory = await _ethicsNumberHistories.GetEthicsNumberHistoryById(id.Value);
            if (ethicsnumberhistory == null)
            {
                return HttpNotFound();
            }

            var animals = await _animals.GetLivingAnimals();
            ViewBag.Animal_Id = new SelectList(animals, "Id", "UniqueAnimalId", ethicsnumberhistory.Animal_Id);
            ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text", ethicsnumberhistory.Ethics_Id);
            return View(ethicsnumberhistory);
        }

        // POST: /EthicsnumberHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Ethics_Id,Animal_Id,Timestamp,AliveStatus")] EthicsNumberHistory ethicsnumberhistory)
        {
            if (ModelState.IsValid)
            {
                await _animals.UpdateEthicsNumberHistoryForAnimalId(ethicsnumberhistory);
                return RedirectToAction("Index", new { id = ethicsnumberhistory.Animal_Id });
            }
            ViewBag.Animal_Id = new SelectList(await _animals.GetLivingAnimals(), "Id", "UniqueAnimalId", ethicsnumberhistory.Animal_Id);
            ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text", ethicsnumberhistory.Ethics_Id);
            return View(ethicsnumberhistory);
        }

        // GET: /EthicsnumberHistory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ethicsnumberhistory = await _ethicsNumberHistories.GetEthicsNumberHistoryById(id.Value);
            if (ethicsnumberhistory == null)
            {
                return HttpNotFound();
            }
            return View(ethicsnumberhistory);
        }

        // POST: /EthicsnumberHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var ethicsnumberhistory = await _ethicsNumberHistories.GetEthicsNumberHistoryById(id);
            await _ethicsNumberHistories.DeleteEthicsNumberHistory(ethicsnumberhistory);
            return RedirectToAction("Index", new { id = ethicsnumberhistory.Animal_Id });
        }

        // GET: /EthicsnumberHistory/RevertToStock/5
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> RevertToStock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }

            return View(animal);
        }

        // POST: /EthicsnumberHistory/RevertToStock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RevertToStock(int id)
        {
            await _animals.ReturnAnimalToStock(id);
            return RedirectToAction("Index", new { id });
        }
    }
}
