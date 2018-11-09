using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
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
        private IRoom _rooms;
        private IAnimalRoomCount _animalRoomCounts;
        private IFeedingGroup _feedingGroups;
        private IAnimal _animals;

        public FeedController()
        {
            this._rooms = new RoomRepo();
            this._animalRoomCounts = new AnimalRoomCountRepo();
            this._feedingGroups = new FeedingGroupRepo();
            this._animals = new AnimalRepo();
        }

        // GET: /Feed/
        public ActionResult Index()
        {
            ViewBag.Num_Animals = _animals.GetLivingAnimals().Count();

            return View(_rooms.GetRooms());
        }

        // GET: /Feed/Groups
        public async Task<ActionResult> Groups(int id)
        {
            ViewBag.NumAppAnimals = _rooms.GetCountOfLivingAnimalsByRoomId(id);
            ViewBag.GMOCount = _rooms.GetCountOfGMOAnimalsByRoomId(id);


            var room = await _rooms.GetRoomById(id);

            ViewBag.RoomName = room.Description;
            ViewBag.Room = room;
            ViewBag.RoomId = id;
            var CountHistory = _animalRoomCounts.GetLastNRoomCountsByRoomId(id);
                

            bool allChecked = true;
            
            foreach (var animal in _rooms.GetLivingAnimalsByRoomId(id))
            {
                if (animal.LastChecked.Date != DateTime.Now.Date)
                {
                    allChecked = false;
                }
            }

            ViewBag.AllChecked = allChecked;

            DateTime startDay = DateTime.Now;

            while (startDay.DayOfWeek != DayOfWeek.Monday)
            {
                startDay = startDay.AddDays(-1);
            }

            ViewBag.DoneToday = CountHistory.Count(m => m.Timestamp.Date == startDay.Date) == 1 ? "1" : "0";

            ViewBag.startDay = startDay;
            ViewBag.CountHistory = CountHistory;
            ViewBag.AnimalsNotInDB = room.NoDBAnimals;
            ViewBag.LastRoomCheckToday = room.NoDBAnimalsLastCheck.HasValue && room.NoDBAnimalsLastCheck.Value.Date == DateTime.Now.Date;

            return View(_feedingGroups.GetFeedingGroupsByRoomId(id));
        }

        // POST: /Feed/Groups/5
        [HttpPost]
        public async Task<ActionResult> Groups(FormCollection values, int id)
        {
            int count = _rooms.GetCountOfLivingAnimalsByRoomId(id);
            int gmocount = _rooms.GetCountOfGMOAnimalsByRoomId(id);

            DateTime startDay = DateTime.Now;

            while (startDay.DayOfWeek != DayOfWeek.Monday)
            {
                startDay = startDay.AddDays(-1);
            }

            var history = new AnimalRoomCount()
            {
                Room_Id = id,
                Timestamp = startDay,
                Count = count,
                GMOCount = gmocount
            };

            await _animalRoomCounts.CreateAnimalRoomCount(history);

            return RedirectToAction("Groups", new { id = id });
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

            ViewBag.CurrentWeight = _animals.GetAnimalsCurrentWeight(id.Value);
                

            return View(animal);
        }
    }
}