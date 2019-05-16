using AnimalDB.Models;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student,Investigator, Technician, Administrator")]
    public class GroupController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IAnimalService _animals;
        private readonly IFeedingGroupService _feedingGroups;
        private readonly IGroupService _groups;

        public GroupController(IAnimalService animals,
                               IFeedingGroupService feedingGroups,
                               IGroupService groups)
        {
            this._animals = animals;
            this._feedingGroups = feedingGroups;
            this._groups = groups;
        }

        // GET: /Group/
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedingGroup group = await _feedingGroups.GetFeedingGroupById(id.Value);
            if (group == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.FeedingGroupId = id.Value;
            ViewBag.FeedingGroupName = group.Description;

            return View(await _groups.GetGroupsByFeedingGroupId(id.Value));
        }

        // GET: /Group/Create
        public ActionResult Create(int id)
        {
            ViewBag.FeedingGroupId = id;
            return View();
        }

        // POST: /Group/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Description")] Group group, int id)
        {
            group.FeedingGroup_Id = id;

            if (ModelState.IsValid)
            {
                await _groups.CreateGroup(group);
                return RedirectToAction("Index", new { id });
            }
            ViewBag.FeedingGroupId = id;
            return View(group);
        }

        // GET: /Group/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await _groups.GetGroupById(id.Value);
            if (group == null)
            {
                return HttpNotFound();
            }

            ViewBag.FeedingGroupId = group.FeedingGroup_Id;
            return View(group);
        }

        // POST: /Group/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description,Investigator_Id,FeedingGroup_Id")] Group group)
        {
            if (ModelState.IsValid)
            {
                await _groups.UpdateGroup(group);
                return RedirectToAction("Index", new { id = group.FeedingGroup_Id });
            }
            ViewBag.FeedingGroupId = group.FeedingGroup_Id;
            return View(group);
        }

        // GET: /Group/AddTo/5
        public async Task<ActionResult> AddTo(int? id, string animalId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Animal animal = await _animals.GetAnimalByUniqueId(animalId);
            Group group = await _groups.GetGroupById(id.Value);
            if (group == null || (!string.IsNullOrEmpty(animalId) && animalId == null))
            {
                return HttpNotFound();
            }

            var model = new AddToGroupViewModel()
            {
                GroupId = group.Id,
                AnimalId = animalId
            };

            ViewBag.FeedingGroupId = group.FeedingGroup_Id;
            ViewBag.AnimalList = group.Animals;
            ViewBag.AnimalId = new SelectList(await _animals.GetLivingAnimals(), "Id", "UniqueAnimalId");
            return View(model);
        }

        // POST: /Group/AddTo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTo([Bind(Include = "AnimalId,GroupId")] AddToGroupViewModel group)
        {
            var _group = await _groups.GetGroupById(group.GroupId);

            if (ModelState.IsValid)
            {
                int animalId = Convert.ToInt32(group.AnimalId);
                if (_group.Animals.Count(m => m.Id == animalId) == 0)
                {
                    _group.Animals.Add(await _animals.GetAnimalById(animalId));
                    await _groups.UpdateGroup(_group);
                    return RedirectToAction("AddTo", new { Id = group.GroupId });
                }
                else
                {
                    Animal animal = await _animals.GetAnimalById(animalId);
                    ModelState.AddModelError("AnimalId", "Animal \"" + animal.UniqueAnimalId + "\" is already in this group");
                }
            }

            ViewBag.FeedingGroupId = _group.FeedingGroup_Id;
            ViewBag.AnimalList = _group.Animals;
            ViewBag.AnimalId = new SelectList(await _animals.GetLivingAnimals(), "Id", "UniqueAnimalId");

            return View(group);
        }

        // GET: /Group/RemoveFrom/5
        public async Task<ActionResult> RemoveFrom(int? id, int? groupid)
        {
            if (id == null || groupid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            await _groups.RemoveAnimalFromGroup(id.Value, groupid.Value);
            return RedirectToAction("AddTo", new { Id = groupid });
        }

        // GET: /Group/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await _groups.GetGroupById(id.Value);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.FeedingGroupId = group.FeedingGroup_Id;
            return View(group);
        }

        // POST: /Group/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Group group = await _groups.GetGroupById(id);
            int feedingGroupId = group.FeedingGroup_Id.Value;
            await _groups.DeleteGroup(group);
            return RedirectToAction("Index", new { id = feedingGroupId });
        }
    }
}
