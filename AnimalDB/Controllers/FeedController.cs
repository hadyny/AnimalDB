﻿using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class FeedController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IRoomService _rooms;
        private readonly IAnimalRoomCountService _animalRoomCounts;
        private readonly IFeedingGroupService _feedingGroups;
        private readonly IAnimalService _animals;

        public FeedController(IRoomService rooms,
                              IAnimalRoomCountService animalRoomCounts,
                              IFeedingGroupService feedingGroups,
                              IAnimalService animals)
        {
            this._rooms = rooms;
            this._animalRoomCounts = animalRoomCounts;
            this._feedingGroups = feedingGroups;
            this._animals = animals;
        }

        //// GET: /Feed/
        //public async Task<ActionResult> Index()
        //{
        //    var animals = await _animals.GetLivingAnimals();
        //    ViewBag.Num_Animals = animals.Count();

        //    return View(await _rooms.GetRooms());
        //}
        // GET: /Feed/
        public async Task<ActionResult> Index()
        {
            var rooms = await _rooms.GetRooms();
            var model = new FeedViewModel();
            var animals = await _animals.GetLivingAnimalsNotCheckedToday();
            var allAnimals = await _animals.GetLivingAnimals();
            model.TotalAnimals = allAnimals.Count();

            foreach (var item in rooms.OrderBy(m => m.Description))
            {
                var row = new FeedRow();

                int animalsNotChecked = animals.Count(m => m.Room_Id == item.Id);
                string _class = "";

                if (!item.NoDBAnimals)
                {
                    if (item.Animals.Count(m => m.DeathDate == null) == 0)
                    {
                        _class = "btn-outline-dark";
                    }
                    else if (animalsNotChecked == item.Animals.Count(m => m.DeathDate == null))
                    {
                        _class = "btn-warning";
                    }
                    else if (animalsNotChecked < item.Animals.Count(m => m.DeathDate == null) && animalsNotChecked > 0)
                    {
                        _class = "btn-info";
                    }
                    else
                    {
                        _class = "btn-navy";
                    }
                }
                else
                {
                    if (item.NoDBAnimalsLastCheck.HasValue && item.NoDBAnimalsLastCheck.Value.Date == DateTime.Now.Date)
                    {
                        _class = "btn-navy";
                    }
                    else
                    {
                        _class = "btn-warning";
                    }
                }

                model.Rooms.Add(new FeedRow() {
                    ClassList = _class,
                    GMOAnimals = item.Animals.Where(m => m.DeathDate == null && m.ApprovalNumber_Id != null).Count(),
                    RoomId = item.Id,
                    RoomName = item.Description,
                    StockAnimals = item.Animals.Where(m => m.DeathDate == null && m.StockAnimal).Count(),
                    TotalAnimals = item.Animals.Where(m => m.DeathDate == null).Count()
                });
            }
            return View(model);
        }

        // GET: /Feed/Groups

        public async Task<ActionResult> Groups(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bool allChecked = true;

            foreach (var animal in await _rooms.GetLivingAnimalsByRoomId(id.Value))
            {
                if (animal.LastChecked.Date != DateTime.Now.Date)
                {
                    allChecked = false;
                }
            }

            DateTime startDay = DateTime.Now;

            while (startDay.DayOfWeek != DayOfWeek.Monday)
            {
                startDay = startDay.AddDays(-1);
            }

            var CountHistory = await _animalRoomCounts.GetLastNRoomCountsByRoomId(id.Value);

            var room = await _rooms.GetRoomById(id.Value);

            var model = new FeedGroupsViewModel()
            {
                NumAppAnimals = await _rooms.GetCountOfLivingAnimalsByRoomId(id.Value),
                GMOCount = await _rooms.GetCountOfGMOAnimalsByRoomId(id.Value),
                Room = room,
                CountHistory = CountHistory,
                AllChecked = allChecked,
                startDay = startDay,
                DoneToday = CountHistory.Count(m => m.Timestamp.Date == startDay.Date) == 1 ? true : false,
                AnimalsNotInDB = room.NoDBAnimals,
                LastRoomCheckToday = room.NoDBAnimalsLastCheck.HasValue && room.NoDBAnimalsLastCheck.Value.Date == DateTime.Now.Date,
                groups = await _feedingGroups.GetFeedingGroupsByRoomId(id.Value)
            };

            return View(model);
        }

        // POST: /Feed/Groups/5
        [HttpPost]
        public async Task<ActionResult> Groups(int id)
        {
            int count = await _rooms.GetCountOfLivingAnimalsByRoomId(id);
            int gmocount = await _rooms.GetCountOfGMOAnimalsByRoomId(id);

            DateTime startDay = DateTime.Now;

            while (startDay.DayOfWeek != DayOfWeek.Monday) { startDay = startDay.AddDays(-1); }

            var history = new AnimalRoomCount()
            {
                Room_Id = id,
                Timestamp = startDay,
                Count = count,
                GMOCount = gmocount
            };

            await _animalRoomCounts.CreateAnimalRoomCount(history);

            return RedirectToAction("Groups", new { id });
        }

        // GET: /Feed/Add/
        public async Task<ActionResult> Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int daystotake = 14;
            var group = await _feedingGroups.GetFeedingGroupById(id.Value);
            foreach (var animal in group.Animals)
            {
                for (DateTime start = DateTime.Now.Date.AddDays(-daystotake); start < DateTime.Now.Date; start = start.AddDays(1))
                {
                    if (animal.Feeds.Count(m => m.Date.Date == start.AddDays(1)) == 0)
                    {
                        animal.Feeds.Add(new Feed() { Animal_Id = animal.Id, Date = start.AddDays(1), FeedingGroupId = id.Value });
                    }
                }
            }
            foreach (var animal in group.Animals)
            {
                animal.Feeds = animal
                                    .Feeds
                                    .OrderByDescending(m => m.Date)
                                    .Take(daystotake)
                                    .OrderBy(m => m.Date)
                                    .ToList();
            }
            group.Animals = group
                                .Animals
                                .OrderBy(m => m.Group_Id)
                                .ToList();
            string groups = "";
            foreach (var _group in group.Groups)
            {
                groups += "<option>" + _group.Description + "</option>";
            }
            ViewBag.Groups = groups;
            ViewBag.daystotake = daystotake;
            return View(group);
        }

        // POST: /Feed/Add/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(FeedingGroup model, string state)
        {
            if (ModelState.IsValid)
            {
                await _feedingGroups.UpdateFeedingGroupPage(model);
                
                if (state == "finish")
                {
                    return RedirectToAction("Groups", new { id = model.Room_Id });
                }
            }
            return RedirectToAction("Add");
        }

        public async Task<ActionResult> MarkAllAnimalsChecked(int roomId)
        {
            await _rooms.MarkAllAnimalsAsCheckedByRoomId(roomId);

            return RedirectToAction("Groups", new { id = roomId });
        }

        public async Task<ActionResult> MarkRoomChecked(int roomId)
        {
            await _rooms.MarkRoomAsChecked(roomId);

            return RedirectToAction("Groups", new { id = roomId });
        }

        // GET: /Feed/Details/5
        [Authorize(Roles = "Student, Investigator, Technician, Administrator, Veterinarian")]
        public async Task<ActionResult> Details(int? id)
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
            ViewBag.AnimalId = id.Value;
            ViewBag.AnimalName = animal.UniqueAnimalId;

            ViewBag.CurrentWeight = await _animals.GetAnimalsCurrentWeight(id.Value);
                

            return View(animal);
        }
    }
}