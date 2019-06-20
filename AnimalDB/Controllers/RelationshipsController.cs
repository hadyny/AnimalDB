using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class RelationshipsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RelationshipsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Relationships/
        public async Task<ActionResult> Index(string id)
        {
            if (id == null)
            {
                return View();
            }

            var model = await _unitOfWork.Animals.GetAnimalByUniqueId(id);

            if (model == null)
            {
                ModelState.AddModelError("", "Could not find animal, please try again");
                return View();
            }

            return View("Index2", model);
        }

        // GET: /Relationships/Details/5
        [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await _unitOfWork.Animals.GetById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }

            ViewBag.AnimalId = animal.Id;
            int AmountCulled = 0;
            int NumMale = 0;
            int NumFemale = 0;

            foreach (var culledPups in _unitOfWork.CulledPups.GetByAnimalId(animal.Id))
            {
                AmountCulled += culledPups.AmountCulled;
                NumMale += culledPups.NumMale ?? 0;
                NumFemale += culledPups.NumFemale ?? 0;
            }

            ViewBag.AmountCulled = AmountCulled;
            ViewBag.NumMale = NumMale;
            ViewBag.NumFemale = NumFemale;

            return View(animal);
        }

        // GET: /Relationships/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Animal animal = await _unitOfWork.Animals.GetById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }

            var animals = _unitOfWork.Animals.GetLiving();

            ViewBag.Parent_Id = new SelectList(animals.Where(m => m.Id != id.Value), "Id", "UniqueAnimalId");
            ViewBag.Child_Id = new SelectList(animals.Where(m => m.Id != id.Value), "Id", "UniqueAnimalId");
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            return View();
        }

        // POST: /Relationships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Models.RelationshipViewModel relationship, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Animal animal = await _unitOfWork.Animals.GetById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }

            var child = await _unitOfWork.Animals.GetById(relationship.Child_Id);
            var parent = await _unitOfWork.Animals.GetById(relationship.Parent_Id);

            if ((relationship.Child_Id != null && relationship.Parent_Id != null) || 
                (relationship.Child_Id == null && relationship.Parent_Id == null))
            {
                ModelState.AddModelError("", "Select either a Parent for " + animal.UniqueAnimalId + ", or a child for " + animal.UniqueAnimalId);
            }
            else if ((relationship.Parent_Id == null && animal.Parents.Contains(child)) || 
                     (relationship.Child_Id == null && animal.Offspring.Contains(parent)))
            {
                ModelState.AddModelError("", "An animal cannot be a parent and an offspring");
            }
            else if (relationship.Parent_Id != null && animal.Parents.Count() >= 2) 
            {
                ModelState.AddModelError("", "An animal cannot have more than 2 parents");
            }
            
            if (ModelState.IsValid)
            {
                if (relationship.Parent_Id == null)
                {   
                    animal.Offspring.Add(child);
                }
                else if (relationship.Child_Id == null)
                {
                    animal.Parents.Add(parent);
                }

                _unitOfWork.Animals.Update(animal);
                await _unitOfWork.Complete();

                return RedirectToAction("Details", new { id = animal.Id });
            }

            var animals = _unitOfWork.Animals.GetLiving();
            var parentList = animals.Where(m => m.Id != id.Value);
            var childList = parentList;
            ViewBag.Parent_Id = new SelectList(parentList, "Id", "UniqueAnimalId", relationship.Parent_Id);
            ViewBag.Child_Id = new SelectList(childList, "Id", "UniqueAnimalId", relationship.Child_Id);
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            return View(relationship);
        }

        // GET: /Relationships/DeleteParent/5
        public async Task<ActionResult> DeleteParent(int? id, int? subject)
        {
            if (id == null || subject == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Animal parent = await _unitOfWork.Animals.GetById(id.Value);
            Animal child = await _unitOfWork.Animals.GetById(subject.Value);

            if (parent == null || child == null)
            {
                return HttpNotFound();
            }

            var model = new Models.RelationshipViewModel()
            {
                Child_Id = subject,
                Child = child,
                Parent_Id = id,
                Parent = parent
            };
            ViewBag.AnimalId = subject.Value;
            return View("Delete", model);
        }

        // POST: /Relationships/DeleteParent/5
        [HttpPost, ActionName("DeleteParent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteParentConfirmed(int id, int subject)
        {
            Animal parent = await _unitOfWork.Animals.GetById(id);
            Animal child = await _unitOfWork.Animals.GetById(subject);
            child.Parents.Remove(parent);
            _unitOfWork.Animals.Update(child);
            await _unitOfWork.Complete();
            return RedirectToAction("Details", new { id = child.Id });
        }

        // GET: /Relationships/DeleteChild/5
        public async Task<ActionResult> DeleteChild(int? id, int? subject)
        {
            if (id == null || subject == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Animal parent = await _unitOfWork.Animals.GetById(subject.Value);
            Animal child = await _unitOfWork.Animals.GetById(id.Value);

            if (parent == null || child == null)
            {
                return HttpNotFound();
            }

            var model = new Models.RelationshipViewModel()
            {
                Child_Id = subject,
                Child = child,
                Parent_Id = id,
                Parent = parent
            };
            ViewBag.AnimalId = subject.Value;
            return View("Delete", model);
        }

        // POST: /Relationships/DeleteChild/5
        [HttpPost, ActionName("DeleteChild")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteChildConfirmed(int id, int subject)
        {
            Animal parent = await _unitOfWork.Animals.GetById(subject);
            Animal child = await _unitOfWork.Animals.GetById(id);
            parent.Offspring.Remove(child);
            _unitOfWork.Animals.Update(parent);
            await _unitOfWork.Complete();
            return RedirectToAction("Details", new { id = parent.Id });
        }
    }
}
