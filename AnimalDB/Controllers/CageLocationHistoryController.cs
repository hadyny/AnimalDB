﻿using AnimalDB.Models;
using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class CageLocationHistoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CageLocationHistoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /CageLocationHistory/
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _unitOfWork.Animals.GetById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;

            return View(_unitOfWork.CageLocationHistories.GetByAnimalId(animal.Id));
        }

        // GET: /CageLocationHistory/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _unitOfWork.Animals.GetById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }

            var thisRoomsRacks = _unitOfWork.Racks.GetByRoomId(animal.Room_Id.Value);
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.CageLocation_Id = new SelectList(await _unitOfWork.CageLocations.Get(), "Id", "Description");
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
                foreach (var entry in _unitOfWork.RackEntries.GetByAnimalId(id.Value))
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

                    _unitOfWork.RackEntries.Insert(newRack);

                    int rackEntryId = newRack.Id;

                    var newCageLocationHistory = new CageLocationHistory()
                    {
                        Animal_Id = cagelocationhistory.Animal_Id,
                        RackEntry_Id = rackEntryId,
                        Timestamp = cagelocationhistory.Timestamp
                    };

                    _unitOfWork.CageLocationHistories.Insert(newCageLocationHistory);
                }
                else
                {
                    var newCageLocationHistory = new CageLocationHistory()
                    {
                        Animal_Id = cagelocationhistory.Animal_Id,
                        CageLocation_Id = cagelocationhistory.CageLocation_Id,
                        Timestamp = cagelocationhistory.Timestamp
                    };

                    _unitOfWork.CageLocationHistories.Insert(newCageLocationHistory);
                }

                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id });
            }
            var animal = _unitOfWork.Animals.GetById(id.Value);
            ViewBag.AnimalName = animal.Result.UniqueAnimalId;
            ViewBag.AnimalId = animal.Result.Id;
            ViewBag.CageLocation_Id = new SelectList(await _unitOfWork.CageLocations.Get(), "Id", "Description");
            return View(cagelocationhistory);
        }

        // GET: /CageLocationHistory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cagelocationhistory = await _unitOfWork.CageLocationHistories.GetById(id.Value);
            if (cagelocationhistory == null)
            {
                return HttpNotFound();
            }

            var roomsRacks = _unitOfWork.Racks.GetByRoomId(cagelocationhistory.Animal.Room_Id.Value);
            ViewBag.AnimalName = cagelocationhistory.Animal.UniqueAnimalId;
            ViewBag.AnimalId = cagelocationhistory.Animal.Id;
            ViewBag.CageLocation_Id = new SelectList(await _unitOfWork.CageLocations.Get(), "Id", "Description", cagelocationhistory.CageLocation_Id);
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
                var location = await _unitOfWork.CageLocationHistories.GetById(id.Value);

                if (location.RackEntry_Id.HasValue)
                {
                    var rackEntry = await _unitOfWork.RackEntries.GetById(location.RackEntry_Id.Value);
                    _unitOfWork.RackEntries.Delete(rackEntry);
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
                    _unitOfWork.RackEntries.Insert(newRack);
                    int rackEntryId = newRack.Id;
                    location.RackEntry_Id = rackEntryId;
                }
                else
                {
                    location.CageLocation_Id = cagelocationhistory.CageLocation_Id;
                }
                _unitOfWork.CageLocationHistories.Update(location);
                await _unitOfWork.Complete();

                return RedirectToAction("Index", new { id = cagelocationhistory.Animal_Id });
            }

            var cagelocation = await _unitOfWork.CageLocationHistories.GetById(cagelocationhistory.Id);
            if (cagelocation == null)
            {
                return HttpNotFound();
            }

            var rackRooms = _unitOfWork.Racks.GetByRoomId(cagelocation.Animal.Room_Id.Value);
            ViewBag.AnimalName = cagelocation.Animal.UniqueAnimalId;
            ViewBag.AnimalId = cagelocation.Animal.Id;
            ViewBag.CageLocation_Id = new SelectList(await _unitOfWork.CageLocations.Get(), "Id", "Description", cagelocation.CageLocation_Id);
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
            var cagelocationhistory = await _unitOfWork.CageLocationHistories.GetById(id.Value);
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
            var cagelocationhistory = await _unitOfWork.CageLocationHistories.GetById(id);
            _unitOfWork.CageLocationHistories.Delete(cagelocationhistory);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id=cagelocationhistory.Animal_Id });
        }

    }
}
