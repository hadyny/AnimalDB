using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class SurgicalNoteController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();

        private readonly IAnimalService _animals;
        private readonly ISurgicalNoteService _surgicalNotes;
        private readonly IInvestigatorService _investigators;
        private readonly IStudentService _students;
        private readonly ISurgeryTypeService _surgeryTypes;
        private readonly IVirusTypeService _virusTypes;
        private readonly IGDTimelineService _gDTimelines;
        private readonly IApprovalNumberService _approvalNumbers;
        private readonly ISurgicalWelfareScoreService _surgicalWelfareScores;


        public SurgicalNoteController(IAnimalService animals,
                                      ISurgicalNoteService surgicalNotes,
                                      IInvestigatorService investigators,
                                      IStudentService students,
                                      ISurgeryTypeService surgeryTypes,
                                      IVirusTypeService virusTypes,
                                      IGDTimelineService gDTimelines,
                                      IApprovalNumberService approvalNumbers,
                                      ISurgicalWelfareScoreService surgicalWelfareScores)
        {
            this._animals = animals;
            this._surgicalNotes = surgicalNotes;
            this._investigators = investigators;
            this._students = students;
            this._surgeryTypes = surgeryTypes;
            this._virusTypes = virusTypes;
            this._gDTimelines = gDTimelines;
            this._approvalNumbers = approvalNumbers;
            this._surgicalWelfareScores = surgicalWelfareScores;
        }

        // GET: /SurgicalNote/
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
            ViewBag.AnimalId = animal.Id;
            ViewBag.AnimalName = animal.UniqueAnimalId;
            return View(await _surgicalNotes.GetSurgicalNotesByAnimalId(animal.Id));
        }

        // GET: /SurgicalNote/Create
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
            
            var model = new SurgicalNote()
            {
                Animal = animal,
                Animal_Id = animal.Id,
                Timestamp = DateTime.Now
            };

            List<string> surgeons = new List<string>
            {
                "HTRU"
            };

            foreach (var user in await _investigators.GetInvestigators())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            foreach (var user in await _students.GetStudents())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            ViewBag.Surgeon = new SelectList(surgeons);
            ViewBag.SurgeryType_Id = new SelectList(await _surgeryTypes.GetSurgeryTypes(), "Id", "Description");
            ViewBag.VirusType_Id = new SelectList(await _virusTypes.GetVirusTypes(), "Id", "Description");
            ViewBag.GDTimeline_Id = new SelectList(await _gDTimelines.GetGDTimelines(), "Id", "Description");

            if (animal.ApprovalNumber_Id != null)
            {
                ViewBag.hsno = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description", animal.ApprovalNumber_Id);
            }
            else
            {
                ViewBag.hsno = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description");
            }

            return View(model);
        }

        // POST: /SurgicalNote/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Animal_Id,Timestamp,SurgeryType_Id,Surgeon,VirusType_Id,GDTimeline_Id")] SurgicalNote surgicalnote, int? id, int? hsno)
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

            surgicalnote.Animal_Id = id.Value;
            var surgeryType = await _surgeryTypes.GetSurgeryTypeById(surgicalnote.SurgeryType_Id);
            if (animal.Sex == Sex.Male && surgeryType.Description == "Plug")
            {
                ModelState.AddModelError("", "This animal is a male. You can only add Plug surgeries to female animals.");
            }

            if (surgeryType.Description == "Plug" && surgicalnote.GDTimeline_Id == null)
            {
                ModelState.AddModelError("GDTimeline_Id", "The GD Timeline is required");
            }

            if (ModelState.IsValid)
            {
                if (animal.ApprovalNumber_Id == null && hsno != null)
                {
                    animal.ApprovalNumber_Id = hsno;
                }

                await _surgicalNotes.CreateSurgicalNote(surgicalnote);
                return RedirectToAction("Index", new { id = surgicalnote.Animal_Id });
            }

            surgicalnote.Animal = animal;

            List<string> surgeons = new List<string>
            {
                "HTRU"
            };

            foreach (var user in await _investigators.GetInvestigators())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            foreach (var user in await _students.GetStudents())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            ViewBag.Surgeon = new SelectList(surgeons, surgicalnote.Surgeon);
            ViewBag.SurgeryType_Id = new SelectList(await _surgeryTypes.GetSurgeryTypes(), "Id", "Description", surgicalnote.SurgeryType_Id);
            ViewBag.VirusType_Id = new SelectList(await _virusTypes.GetVirusTypes(), "Id", "Description", surgicalnote.VirusType_Id);
            ViewBag.GDTimeline_Id = new SelectList(await _gDTimelines.GetGDTimelines(), "Id", "Description", surgicalnote.GDTimeline_Id);
            ViewBag.hsno= new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description", hsno);
            return View(surgicalnote);
        }

        // GET: /SurgicalNote/FollowUps/5
        public async Task<ActionResult> Followups(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurgicalNote surgicalnote = await _surgicalNotes.GetSurgicalNoteById(id.Value);
            if (surgicalnote == null)
            {
                return HttpNotFound();
            }

            ViewBag.SurgicalNote_Id = id.Value;
            ViewBag.Animal_Id = surgicalnote.Animal_Id;

            return View(surgicalnote.WellfareScores.OrderBy(m => m.Timestamp));
        }


        // GET: /SurgicalNote/FollowUp/5
        public async Task<ActionResult> Followup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurgicalNote surgicalnote = await _surgicalNotes.GetSurgicalNoteById(id.Value);
            if (surgicalnote == null)
            {
                return HttpNotFound();
            }

            var model = new SurgicalWelfareScore()
            {
                SurgicalNote_Id = id.Value,
                Timestamp = DateTime.Now
            };

            var lastFollowup = surgicalnote
                                    .WellfareScores
                                    .Where(m => m.Timestamp.Date == DateTime.Now.AddDays(-1).Date);

            if (lastFollowup.Count() != 0)
            {
                model.Day = lastFollowup.First().Day + 1;
                model.BodyWtYesterday = lastFollowup.First().BodyWtToday;
                model.StartWeight = lastFollowup.First().CurrentWeight;
            }

            return View(model);
        }

        // POST: /SurgicalNote/FollowUp/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Followup(SurgicalWelfareScore surgicalWelfareScore)
        {
            if (ModelState.IsValid)
            {
                var surgicalNote = await _surgicalNotes.GetSurgicalNoteById(surgicalWelfareScore.SurgicalNote_Id);
                surgicalNote.WellfareScores.Add(surgicalWelfareScore);
                await _surgicalNotes.UpdateSurgicalNote(surgicalNote);
                return RedirectToAction("Followups", new { id = surgicalWelfareScore.SurgicalNote_Id });
            }

            return View(surgicalWelfareScore);
        }

        // GET: /SurgicalNote/EditFollowup/5
        public async Task<ActionResult> EditFollowup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SurgicalWelfareScore surgicalWelfareScore = await _surgicalWelfareScores.GetSurgicalWelfareScoreById(id.Value);

            if (surgicalWelfareScore == null)
            {
                return HttpNotFound();
            }

            return View(surgicalWelfareScore);
        }


        // POST: /SurgicalNote/EditFollowup/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditFollowup(SurgicalWelfareScore surgicalWelfareScore)
        {
            if (ModelState.IsValid)
            {
                await _surgicalWelfareScores.UpdateSurgicalWelfareScore(surgicalWelfareScore);
                return RedirectToAction("Followups", new { id = surgicalWelfareScore.SurgicalNote_Id });
            }

            return View(surgicalWelfareScore);
        }

        // GET: /SurgicalNote/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurgicalNote surgicalnote = await _surgicalNotes.GetSurgicalNoteById(id.Value);
            if (surgicalnote == null)
            {
                return HttpNotFound();
            }

            List<string> surgeons = new List<string>
            {
                "HTRU"
            };

            foreach (var user in await _investigators.GetInvestigators())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            foreach (var user in await _students.GetStudents())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            ViewBag.Surgeon = new SelectList(surgeons, surgicalnote.Surgeon);
            ViewBag.SurgeryType_Id = new SelectList(await _surgeryTypes.GetSurgeryTypes(), "Id", "Description", surgicalnote.SurgeryType_Id);
            ViewBag.VirusType_Id = new SelectList(await _virusTypes.GetVirusTypes(), "Id", "Description", surgicalnote.VirusType_Id);
            ViewBag.GDTimeline_Id = new SelectList(await _gDTimelines.GetGDTimelines(), "Id", "Description", surgicalnote.GDTimeline_Id);
            return View(surgicalnote);
        }

        // POST: /SurgicalNote/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Animal_Id,Timestamp,Surgeon,SurgeryType_Id,VirusType_Id,GDTimeline_Id")] SurgicalNote surgicalnote)
        {
            var animal = await _animals.GetAnimalById(surgicalnote.Animal_Id);
            surgicalnote.SurgeryType = await _surgeryTypes.GetSurgeryTypeById(surgicalnote.SurgeryType_Id);
            if (animal.Sex == Sex.Male && surgicalnote.SurgeryType.Description == "Plug")
            {
                ModelState.AddModelError("", "This animal is a male. You can only add Plug surgeries to female animals.");
            }

            if (surgicalnote.SurgeryType.Description == "Plug" && surgicalnote.GDTimeline_Id == null)
            {
                ModelState.AddModelError("GDTimeline_Id", "The GD Timeline is required");
            }

            if (ModelState.IsValid)
            {
                await _surgicalNotes.UpdateSurgicalNote(surgicalnote);
                return RedirectToAction("Index", new { id = surgicalnote.Animal_Id });
            }

            List<string> surgeons = new List<string>
            {
                "HTRU"
            };

            foreach (var user in await _investigators.GetInvestigators())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            foreach (var user in await _students.GetStudents())
            {
                surgeons.Add(user.FirstName + " " + user.LastName);
            }

            ViewBag.Surgeon = new SelectList(surgeons, surgicalnote.Surgeon);

            surgicalnote.Animal = animal;
            ViewBag.SurgeryType_Id = new SelectList(await _surgeryTypes.GetSurgeryTypes(), "Id", "Description", surgicalnote.SurgeryType_Id);
            ViewBag.VirusType_Id = new SelectList(await _virusTypes.GetVirusTypes(), "Id", "Description", surgicalnote.VirusType_Id);
            ViewBag.GDTimeline_Id = new SelectList(await _gDTimelines.GetGDTimelines(), "Id", "Description", surgicalnote.GDTimeline_Id);
            return View(surgicalnote);
        }

        // GET: /SurgicalNote/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurgicalNote surgicalnote = await _surgicalNotes.GetSurgicalNoteById(id.Value);
            if (surgicalnote == null)
            {
                return HttpNotFound();
            }
            return View(surgicalnote);
        }

        // POST: /SurgicalNote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SurgicalNote surgicalnote = await _surgicalNotes.GetSurgicalNoteById(id);
            await _surgicalNotes.DeleteSurgicalNote(surgicalnote);
            return RedirectToAction("Index", new { id = surgicalnote.Animal_Id });
        }
    }
}
