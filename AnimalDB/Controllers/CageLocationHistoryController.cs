using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AnimalDB.Models;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class CageLocationHistoryController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IAnimal _animals;
        private ICageLocationHistory _cageLocationHistories;
        private ICageLocation _cageLocations;
        private IRack _racks;
        private IRackEntry _rackEntries;

        public CageLocationHistoryController()
        {
            this._animals = new AnimalRepo();
            this._cageLocationHistories = new CageLocationHistoryRepo();
            this._cageLocations = new CageLocationRepo();
            this._racks = new RackRepo();
            this._rackEntries = new RackEntryRepo();
        }

        // GET: /CageLocationHistory/
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

            return View(_cageLocationHistories.GetCageLocationHistoryByAnimalId(animal.Id));
        }

        // GET: /CageLocationHistory/Create
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

            var thisRoomsRacks = _racks.GetRacksByRoomId(animal.Room_Id.Value);
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.CageLocation_Id = new SelectList(_cageLocations.GetCageLocations(), "Id", "Description");
            ViewBag.Rack_Id = new SelectList(thisRoomsRacks, "Id", "Reference_Id");

            var model = new Models.CageLocationHistoryViewModel
            {
                Racks = thisRoomsRacks
            };

            char c;
            foreach (var rack in model.Racks)
            {
                c = 'A';
                for (int i = 0; i < rack.Width; i++) {
                    for (int j = 1; j <= rack.Height; j++) {

                        if (rack.RackEntries.Count(m => m.LocationReferenceX == j && m.LocationReferenceY == c.ToString() && m.IsCurrent) == 0)
                        {
                            rack.RackEntries.Add(new RackEntry()
                            {
                                Id = (i * rack.Width + j),
                                LocationReferenceX = j,
                                LocationReferenceY = c.ToString(),
                                Rack_Id = rack.Id,
                                IsCurrent = true
                            });
                        }
                    }
                    c++;
                }
            }

            return View(model);
        }

        // POST: /CageLocationHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CageLocationHistoryViewModel cagelocationhistory, int? id)
        {
            cagelocationhistory.Timestamp = DateTime.Now;
            cagelocationhistory.Animal_Id = id.Value;

            if (ModelState.IsValid)
            {
                foreach (var entry in _rackEntries.GetRackEntriesByAnimalId(id.Value))
                {
                    entry.IsCurrent = false;
                }

                if (cagelocationhistory.CageLocation_Id == null)
                {
                    var newRack = new RackEntry() {
                        LocationReferenceX = Convert.ToInt16(cagelocationhistory.RackEntry_X),
                        LocationReferenceY = cagelocationhistory.RackEntry_Y,
                        Rack_Id = Convert.ToInt16(cagelocationhistory.SelectedRack),
                        Animal_Id = cagelocationhistory.Animal_Id,
                        IsCurrent = true
                    };

                    await _rackEntries.CreateRackEntry(newRack);

                    int rackEntryId = newRack.Id;

                    var newCageLocationHistory = new CageLocationHistory()
                    {
                        Animal_Id = cagelocationhistory.Animal_Id,
                        RackEntry_Id = rackEntryId,
                        Timestamp = cagelocationhistory.Timestamp
                    };

                    await _cageLocationHistories.CreateCageLocationHistory(newCageLocationHistory);
                }
                else
                {
                    var newCageLocationHistory = new CageLocationHistory()
                    {
                        Animal_Id = cagelocationhistory.Animal_Id,
                        CageLocation_Id = cagelocationhistory.CageLocation_Id,
                        Timestamp = cagelocationhistory.Timestamp
                    };

                    await _cageLocationHistories.CreateCageLocationHistory(newCageLocationHistory);
                }

                return RedirectToAction("Index", new { id = id });
            }
            var animal = _animals.GetAnimalById(id.Value);
            ViewBag.AnimalName = animal.Result.UniqueAnimalId;
            ViewBag.AnimalId = animal.Result.Id;
            ViewBag.CageLocation_Id = new SelectList(_cageLocations.GetCageLocations(), "Id", "Description");
            return View(cagelocationhistory);
        }

        // GET: /CageLocationHistory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cagelocationhistory = await new CageLocationHistoryRepo().GetCageLocationHistoryById(id.Value);
            if (cagelocationhistory == null)
            {
                return HttpNotFound();
            }

            var roomsRacks = _racks.GetRacksByRoomId(cagelocationhistory.Animal.Room_Id.Value).ToList();
            ViewBag.AnimalName = cagelocationhistory.Animal.UniqueAnimalId;
            ViewBag.AnimalId = cagelocationhistory.Animal.Id;
            ViewBag.CageLocation_Id = new SelectList(_cageLocations.GetCageLocations(), "Id", "Description", cagelocationhistory.CageLocation_Id);
            ViewBag.Rack_Id = new SelectList(roomsRacks, 
                                                        "Id", 
                                                        "Reference_Id", 
                                                        cagelocationhistory.RackEntry != null ? cagelocationhistory.RackEntry.Rack_Id as int? : null);

            var model = new CageLocationHistoryViewModel
            {
                Animal_Id = cagelocationhistory.Animal_Id,
                Timestamp = cagelocationhistory.Timestamp,
                Racks = roomsRacks
            };

            char c;
            foreach (var rack in model.Racks)
            {
                c = 'A';
                for (int i = 0; i < rack.Width; i++)
                {
                    for (int j = 1; j <= rack.Height; j++)
                    {

                        if (rack.RackEntries.Count(m => m.LocationReferenceX == j && m.LocationReferenceY == c.ToString() && m.IsCurrent) == 0)
                        {
                            rack.RackEntries.Add(new RackEntry()
                            {
                                Id = (i * rack.Width + j),
                                LocationReferenceX = j,
                                LocationReferenceY = c.ToString(),
                                Rack_Id = rack.Id,
                                IsCurrent = true
                            });
                        }
                    }
                    c++;
                }
            }
            return View(model);
        }

        // POST: /CageLocationHistory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CageLocationHistoryViewModel cagelocationhistory, int? id)
        {            
            if (ModelState.IsValid)
            {
                var location = await _cageLocationHistories.GetCageLocationHistoryById(id.Value);

                if (location.RackEntry_Id.HasValue)
                {
                    var rackEntry = await _rackEntries.GetRackEntryById(location.RackEntry_Id.Value);
                    await _rackEntries.DeleteRackEntry(rackEntry);
                }
                else
                {
                    location.CageLocation_Id = null;
                }

                if (cagelocationhistory.CageLocation_Id == null)
                {                    
                    var newRack = new RackEntry()
                    {
                        LocationReferenceX = Convert.ToInt16(cagelocationhistory.RackEntry_X),
                        LocationReferenceY = cagelocationhistory.RackEntry_Y,
                        Rack_Id = Convert.ToInt16(cagelocationhistory.SelectedRack),
                        Animal_Id = cagelocationhistory.Animal_Id,
                        IsCurrent = true
                    };
                    await _rackEntries.CreateRackEntry(newRack);
                    int rackEntryId = newRack.Id;
                    location.RackEntry_Id = rackEntryId;
                }
                else
                {
                    location.CageLocation_Id = cagelocationhistory.CageLocation_Id;
                }
                await _cageLocationHistories.UpdateCageLocationHistory(location);

                return RedirectToAction("Index", new { id = cagelocationhistory.Animal_Id });
            }

            var cagelocation = await _cageLocationHistories.GetCageLocationHistoryById(cagelocationhistory.Id);
            if (cagelocation == null)
            {
                return HttpNotFound();
            }

            var rackRooms = _racks.GetRacksByRoomId(cagelocation.Animal.Room_Id.Value);
            ViewBag.AnimalName = cagelocation.Animal.UniqueAnimalId;
            ViewBag.AnimalId = cagelocation.Animal.Id;
            ViewBag.CageLocation_Id = new SelectList(_cageLocations.GetCageLocations(), "Id", "Description", cagelocation.CageLocation_Id);
            ViewBag.Rack_Id = new SelectList(rackRooms, "Id", "Reference_Id");
            var model = new CageLocationHistoryViewModel
            {
                Racks = rackRooms
            };

            char c;
            foreach (var rack in model.Racks)
            {
                c = 'A';
                for (int i = 0; i < rack.Width; i++)
                {
                    for (int j = 1; j <= rack.Height; j++)
                    {

                        if (rack.RackEntries.Count(m => m.LocationReferenceX == j && m.LocationReferenceY == c.ToString()) == 0)
                        {
                            rack.RackEntries.Add(new RackEntry()
                            {
                                Id = (i * rack.Width + j),
                                LocationReferenceX = j,
                                LocationReferenceY = c.ToString(),
                                Rack_Id = rack.Id
                            });
                        }
                    }
                    c++;
                }
            }
            return View(model);
        }

        // GET: /CageLocationHistory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cagelocationhistory = await _cageLocationHistories.GetCageLocationHistoryById(id.Value);
            if (cagelocationhistory == null)
            {
                return HttpNotFound();
            }
            return View(cagelocationhistory);
        }

        // POST: /CageLocationHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var cagelocationhistory = await _cageLocationHistories.GetCageLocationHistoryById(id);
            await _cageLocationHistories.DeleteCageLocationHistory(cagelocationhistory);
            return RedirectToAction("Index", new { id=cagelocationhistory.Animal_Id });
        }

    }
}
