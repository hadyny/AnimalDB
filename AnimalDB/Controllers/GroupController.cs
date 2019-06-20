using AnimalDB.Models;
using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
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
        private readonly IUnitOfWork _unitOfWork;

        public GroupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Group/
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedingGroup group = await _unitOfWork.FeedingGroups.GetById(id.Value);
            if (group == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.FeedingGroupId = id.Value;
            ViewBag.FeedingGroupName = group.Description;

            return View(_unitOfWork.Groups.GetByFeedingGroupId(id.Value));
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
                _unitOfWork.Groups.Insert(group);
                await _unitOfWork.Complete();
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
            Group group = await _unitOfWork.Groups.GetById(id.Value);
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
                _unitOfWork.Groups.Update(group);
                await _unitOfWork.Complete();
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
            Group group = await _unitOfWork.Groups.GetById(id.Value);
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
            ViewBag.AnimalId = new SelectList(_unitOfWork.Animals.GetLiving(), "Id", "UniqueAnimalId");
            return View(model);
        }

        // POST: /Group/AddTo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTo([Bind(Include = "AnimalId,GroupId")] AddToGroupViewModel group)
        {
            var _group = await _unitOfWork.Groups.GetById(group.GroupId);

            if (ModelState.IsValid)
            {
                int animalId = Convert.ToInt32(group.AnimalId);
                if (_group.Animals.Count(m => m.Id == animalId) == 0)
                {
                    _group.Animals.Add(await _unitOfWork.Animals.GetById(animalId));
                    _unitOfWork.Groups.Update(_group);
                    await _unitOfWork.Complete();
                    return RedirectToAction("AddTo", new { Id = group.GroupId });
                }
                else
                {
                    Animal animal = await _unitOfWork.Animals.GetById(animalId);
                    ModelState.AddModelError("AnimalId", "Animal \"" + animal.UniqueAnimalId + "\" is already in this group");
                }
            }

            ViewBag.FeedingGroupId = _group.FeedingGroup_Id;
            ViewBag.AnimalList = _group.Animals;
            ViewBag.AnimalId = new SelectList(_unitOfWork.Animals.GetLiving(), "Id", "UniqueAnimalId");

            return View(group);
        }

        // GET: /Group/RemoveFrom/5
        public async Task<ActionResult> RemoveFrom(int? id, int? groupid)
        {
            if (id == null || groupid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            await _unitOfWork.Groups.RemoveAnimalFromGroup(id.Value, groupid.Value);
            await _unitOfWork.Complete();
            return RedirectToAction("AddTo", new { Id = groupid });
        }

        // GET: /Group/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await _unitOfWork.Groups.GetById(id.Value);
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
            Group group = await _unitOfWork.Groups.GetById(id);
            int feedingGroupId = group.FeedingGroup_Id.Value;
            _unitOfWork.Groups.Delete(group);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id = feedingGroupId });
        }
    }
}
