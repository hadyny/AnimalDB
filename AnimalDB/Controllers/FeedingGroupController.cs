using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class FeedingGroupController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IFeedingGroupService _feedingGroups;
        private readonly IRoomService _rooms;
        private readonly IInvestigatorService _investigators;
        private readonly IGroupService _groups;
        private readonly IAnimalService _animals;

        public FeedingGroupController(IFeedingGroupService feedingGroups,
                                      IRoomService rooms,
                                      IInvestigatorService investigators,
                                      IGroupService groups,
                                      IAnimalService animals)
        {
            this._feedingGroups = feedingGroups;
            this._rooms = rooms;
            this._investigators = investigators;
            this._groups = groups;
            this._animals = animals;
        }

        // GET: /FeedingGroup/
        public async Task<ActionResult> Index()
        {
            return View(await _feedingGroups.GetFeedingGroups());
        }

        [Authorize(Roles = "Administrator,Technician,Investigator")]
        // GET: /FeedingGroup/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName");
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description");
            return View();
        }

        [Authorize(Roles = "Administrator,Technician,Investigator")]
        // POST: /FeedingGroup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description, Investigator_Id, Room_Id")] FeedingGroup feedinggroup)
        {
            if (User.IsInRole("Investigator"))
            {
                var investigator = await _investigators.GetInvestigatorByUsername(User.Identity.Name);
                feedinggroup.Investigator_Id = investigator.Id;
            }

            if (ModelState.IsValid)
            {
                await _feedingGroups.CreateFeedingGroup(feedinggroup);
                return RedirectToAction("Index");
            }
            ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName");
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", feedinggroup.Room_Id);
            return View(feedinggroup);
        }

        [Authorize(Roles = "Administrator,Technician,Investigator")]
        // GET: /FeedingGroup/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedingGroup feedinggroup = await _feedingGroups.GetFeedingGroupById(id.Value);
            if (feedinggroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName", feedinggroup.Investigator_Id);
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", feedinggroup.Room_Id);
            return View(feedinggroup);
        }

        [Authorize(Roles = "Administrator,Technician,Investigator")]
        // POST: /FeedingGroup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description,FirstFeed,Investigator_Id,Room_Id")] FeedingGroup feedinggroup)
        {
            if (User.IsInRole("Investigator"))
            {
                var investigator = await _investigators.GetInvestigatorByUsername(User.Identity.Name);
                feedinggroup.Investigator_Id = investigator.Id;
            }
           
            if (ModelState.IsValid)
            {
                await _feedingGroups.UpdateFeedingGroup(feedinggroup);
                return RedirectToAction("Index");
            }
            ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName", feedinggroup.Investigator_Id);
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", feedinggroup.Room_Id);
            return View(feedinggroup);
        }
        
        // GET: /FeedingGroup/Group/5
        public async Task<ActionResult> Group(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedingGroup feedinggroup = await _feedingGroups.GetFeedingGroupById(id.Value);
            if (feedinggroup == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Group_Id = new SelectList(await _groups.GetGroupsByFeedingGroupId(id.Value),
                                            "Id", "Description");

            var model = new Models.CreateGroupViewModel()
            {
                Id = id.Value,
                Description = feedinggroup.Description,
                AnimalGroups = feedinggroup
                                    .Animals
                                    .Select(m => new Models.AnimalGroup() {
                                                Id = m.Id,
                                                UniqueAnimalId = m.UniqueAnimalId,
                                                Group_Id = m.Group_Id
                                            })
                                    .ToList()
            };

            var tmp = new SelectList(await _groups.GetGroupsByFeedingGroupId(id.Value), "Id", "Description");

            ViewBag.Groups = tmp;

            return View(model);
        }
        
        // POST: /FeedingGroup/Group/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Group(Models.CreateGroupViewModel group)
        {
            if (ModelState.IsValid)
            {
                foreach (var animal in group.AnimalGroups)
                {
                    var _animal = await _animals.GetAnimalById(animal.Id.Value);
                    _animal.Group_Id = animal.Group_Id;
                    await _animals.UpdateAnimal(_animal);
                }

                return RedirectToAction("Index");
            }
            return View(group);
        }

        [Authorize(Roles = "Administrator,Technician,Investigator")]
        // GET: /FeedingGroup/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedingGroup feedinggroup = await _feedingGroups.GetFeedingGroupById(id.Value);
            if (feedinggroup == null)
            {
                return HttpNotFound();
            }
            return View(feedinggroup);
        }

        [Authorize(Roles = "Administrator,Technician,Investigator")]
        // POST: /FeedingGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FeedingGroup feedinggroup = await _feedingGroups.GetFeedingGroupById(id);

            await _feedingGroups.DeleteFeedingGroup(feedinggroup);
            return RedirectToAction("Index");
        }

        // GET: /FeedingGroup/Add/5
        public async Task<ActionResult> Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var feedinggroup = await _feedingGroups.GetFeedingGroupById(id.Value);
            if (feedinggroup == null)
            {
                return HttpNotFound();
            }

            var model = new Models.AddToFeedingGroupViewModel()
            {
                Id = feedinggroup.Id,
                ExistingAnimals = await _feedingGroups.GetAnimalsInFeedingGroup(feedinggroup.Id)
            };

            ViewBag.Animal_Id = new SelectList(feedinggroup
                                                        .Room
                                                        .Animals
                                                        .Where(m => m.CauseOfDeath == null && m.FeedingGroup_Id == null)
                                                        .OrderBy(m => m.UniqueAnimalId.Length), 
                                                "Id", "UniqueAnimalId");
            ViewBag.Group_Id = new SelectList(await _groups.GetGroups(), "Id", "Description");

            return View(model);
        }

        // POST: /FeedingGroup/Add/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(Models.AddToFeedingGroupViewModel addToFeedingGroupModel)
        {
            var feedingGroup = await _feedingGroups.GetAnimalsInFeedingGroup(addToFeedingGroupModel.Id);

            if (ModelState.IsValid && feedingGroup.Count() < 6)
            {
                var addAnimal = await _animals.GetAnimalById(addToFeedingGroupModel.Animal_Id.Value);
                addAnimal.FeedingGroup_Id = addToFeedingGroupModel.Id;
                await _animals.UpdateAnimal(addAnimal);
                return RedirectToAction("Add", addToFeedingGroupModel.Id);
            }
            else if (feedingGroup.Count() > 5)
            {
                ModelState.AddModelError("", "You can only have up to six animals in a feeding group.");
            }
            var animals = await _animals.GetLivingAnimals();
            addToFeedingGroupModel.ExistingAnimals = await _feedingGroups.GetAnimalsInFeedingGroup(addToFeedingGroupModel.Id);
            ViewBag.Animal_Id = new SelectList(animals.Where(m => m.FeedingGroup_Id == null), "Id", "UniqueAnimalId", null);
            ViewBag.Group_Id = new SelectList(await _groups.GetGroups(), "Id", "Description", null);
            return View("Add", addToFeedingGroupModel);
        }

        // GET: /Group/RemoveFrom/5
        public async Task<ActionResult> RemoveFrom(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int? group = await _feedingGroups.GetFeedingGroupByAnimalId(id.Value);
            await _feedingGroups.RemoveAnimalFromFeedingGroup(id.Value);

            return RedirectToAction("Add", new { Id = group });
        }
    }
}
