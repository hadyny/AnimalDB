using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Implementations;
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
    public class MedicationController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IAnimal _animals;
        private IMedication _medications;
        private IMedicationType _medicationTypes;
        private IMedicationFollowUp _medicationFollowUps;
        private INotification _notifications;

        public MedicationController()
        {
            this._animals = new AnimalRepo();
            this._medications = new MedicationRepo();
            this._medicationTypes = new MedicationTypeRepo();
            this._medicationFollowUps = new MedicationFollowUpRepo();
            this._notifications = new NotificationRepo();
        }


        // GET: /Medication/
        public async Task<ActionResult> Index(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ViewBag.AnimalId = id.Value;
            var animal = await _animals.GetAnimalById(id.Value);
            ViewBag.AnimalName = animal.UniqueAnimalId;
            return View(_medications.GetMedicationByAnimalId(id.Value));
        }

        // GET: /Medication/Create
        public async Task<ActionResult> Create(int? id, string returnUrl, int? report)
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

            ICollection<AnimalUser> users = new List<AnimalUser>
            {
                animal.Room.Technician
            };

            if (animal.Investigator_Id != null)
            {
                foreach (var user in animal.Investigator.Students)
                {
                    users.Add(user);
                }
            }

            ViewBag.WhoToNotify_Id = new SelectList(users, "Id", "FullName");
            ViewBag.MedicationType_Id = new SelectList(_medicationTypes.GetMedicationTypes(), "Id", "Description");
            
            var model = new Medication() { Animal = animal, Timestamp = DateTime.Now };
            return View(model);
        }

        // POST: /Medication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Timestamp,Dosage,Rate,Frequency,FrequencyValue,Duration,DurationValue,Comments,MedicationType_Id,WhoToNotify_Id")] Medication medication, int? id, string returnUrl, int? report)
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

            medication.Animal_Id = id.Value;
            medication.IncidentReport_Id = report.HasValue ? (int?)report.Value : null;
            
            if (ModelState.IsValid)
            {
                await _medications.CreateMedication(medication);

                await createReminders(medication);

                if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(returnUrl + "?medid=" + medication.Id);
                }
                return RedirectToAction("Index", new { id=id });
            }
            ICollection<AnimalUser> users = new List<AnimalUser>
            {
                animal.Room.Technician
            };
            foreach (var user in animal.Investigator.Students)
            {
                users.Add(user);
            }
            ViewBag.WhoToNotify_Id = new SelectList(users, "Id", "FullName", medication.WhoToNotify_Id);
            ViewBag.MedicationType_Id = new SelectList(_medicationTypes.GetMedicationTypes(), "Id", "Description", medication.MedicationType_Id);
            return View(medication);
        }

        // GET: /Medication/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = await _medications.GetMedicationById(id.Value);
            if (medication == null)
            {
                return HttpNotFound();
            }
            var animal = await _animals.GetAnimalById(medication.Animal_Id);
            ICollection<AnimalUser> users = new List<AnimalUser>
            {
                animal.Room.Technician
            };
            foreach (var user in animal.Investigator.Students)
            {
                users.Add(user);
            }
            ViewBag.WhoToNotify_Id = new SelectList(users, "Id", "FullName", medication.WhoToNotify_Id);
            ViewBag.MedicationType_Id = new SelectList(_medicationTypes.GetMedicationTypes(), "Id", "Description", medication.MedicationType_Id);
            return View(medication);
        }

        // POST: /Medication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Timestamp,Current,Animal_Id,Dosage,Rate,Frequency,FrequencyValue,Duration,DurationValue,Comments,MedicationType_Id,IncidentReport_Id,WhoToNotify_Id")] Medication medication)
        {
            if (ModelState.IsValid)
            {
                await _medications.UpdateMedication(medication);
                await createReminders(medication);
                return RedirectToAction("Index", new { id = medication.Animal_Id });
            }
            var animal = await _animals.GetAnimalById(medication.Animal_Id);
            ICollection<AnimalUser> users = new List<AnimalUser>
            {
                animal.Room.Technician
            };
            foreach (var user in animal.Investigator.Students)
            {
                users.Add(user);
            }
            ViewBag.WhoToNotify_Id = new SelectList(users, "Id", "FullName", medication.WhoToNotify_Id);
            ViewBag.MedicationType_Id = new SelectList(_medicationTypes.GetMedicationTypes(), "Id", "Description", medication.MedicationType_Id);
            return View(medication);
        }

        // GET: /Medication/Delete/5
        public async Task<ActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medication medication = await _medications.GetMedicationById(id.Value);
            if (medication == null)
            {
                return HttpNotFound();
            }
            return View(medication);
        }

        // POST: /Medication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, string returnUrl)
        {
            Medication medication = await _medications.GetMedicationById(id);
            await _medications.DeleteMedication(medication);

            if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return this.Redirect(returnUrl);
                }

            return RedirectToAction("Index", new { id = medication.Animal_Id });
        }


        // GET: /SurgicalNote/FollowUps/5
        public async Task<ActionResult> Followups(int? id)
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
            ViewBag.Animal_Id = id.Value;

            return View(_medicationFollowUps.GetMedicationFollowUpByAnimalId(id.Value));
        }


        // GET: /SurgicalNote/FollowUp/5
        public async Task<ActionResult> Followup(int? id)
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

            var model = new MedicationFollowUp()
            {
                Timestamp = DateTime.Now
            };

            ViewBag.Animal_Id = animal.Id;
            ViewBag.AnimalName = animal.UniqueAnimalId;

            var medications = animal
                                .Medications
                                .OrderByDescending(m => m.Timestamp)
                                .ToList()
                                .Select(s => new 
                                { 
                                    medicationId = s.Id,
                                    description = s.Timestamp.ToShortDateString() + ": " + s.MedicationType.Description
                                });

            ViewBag.Medication_Id = new SelectList(medications, "medicationId", "description");

            string medid = Request["medid"];

            if (!string.IsNullOrEmpty(medid) && medications.Count(m => m.medicationId.ToString() == medid) != 0)
            {
                ViewBag.Medication_Id = new SelectList(medications, "medicationId", "description", medid);
            }
            else
            {
                ViewBag.Medication_Id = new SelectList(medications, "medicationId", "description");
            }

            return View(model);
        }

        // POST: /SurgicalNote/FollowUp/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Followup(MedicationFollowUp medicalFollowUp, int? id)
        {
            if (ModelState.IsValid)
            {
                await _medicationFollowUps.CreateMedicationFollowUp(medicalFollowUp);
                // Remove relevant notification
                var notifications = _notifications.GetNotificationsByMedicationId(medicalFollowUp.Medication_Id);

                if (notifications.Count(m => m.NotificationDate < DateTime.Now.AddMinutes(5)) != 0)
                {
                    await _notifications.DeleteNotification(notifications.First());
                }
                
                return RedirectToAction("Followups", new { id = id });
            }
            var animal = await _animals.GetAnimalById(id.Value);
            ViewBag.Animal_Id = id;
            ViewBag.AnimalName = animal.UniqueAnimalId;
            var medications = animal
                                .Medications
                                .OrderByDescending(m => m.Timestamp)
                                .Select(s => new
                                {
                                    medicationId = s.Id,
                                    description = s.Timestamp.ToShortDateString() + ": " + s.MedicationType.Description
                                });

            ViewBag.Medication_Id = new SelectList(medications, "medicationId", "description", medicalFollowUp.Medication_Id);

            return View(medicalFollowUp);
        }

        // GET: /SurgicalNote/EditFollowup/5
        public async Task<ActionResult> EditFollowup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var medicalFollowUp = await _medicationFollowUps.GetMedicationFollowUpById(id.Value);

            if (medicalFollowUp == null)
            {
                return HttpNotFound();
            }

            var animal = await _animals.GetAnimalById(medicalFollowUp.Medication.Animal_Id);

            ViewBag.Animal_Id = animal.Id;
            var medications = animal
                                .Medications
                                .OrderByDescending(m => m.Timestamp)
                                .Select(s => new
                                {
                                    medicationId = s.Id,
                                    description = s.Timestamp.ToShortDateString() + ": " + s.MedicationType.Description
                                });

            ViewBag.Medication_Id = new SelectList(medications, "medicationId", "description", medicalFollowUp.Medication_Id);

            return View(medicalFollowUp);
        }


        // POST: /SurgicalNote/EditFollowup/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditFollowup(MedicationFollowUp medicalFollowUp, int id)
        {
            if (ModelState.IsValid)
            {
                await _medicationFollowUps.UpdateMedicationFollowUp(medicalFollowUp);
                var animalId = _medications.GetMedicationById(medicalFollowUp.Medication_Id).Result.Animal_Id;
                return RedirectToAction("Followups", new { id = animalId });
            }

            var animal = _medicationFollowUps.GetMedicationFollowUpById(id).Result.Medication.Animal;

            ViewBag.Animal_Id = animal.Id;
            
            var medications = animal
                                .Medications
                                .OrderByDescending(m => m.Timestamp)
                                .Select(s => new
                                {
                                    medicationId = s.Id,
                                    description = s.Timestamp.ToShortDateString() + ": " + s.MedicationType.Description
                                });

            ViewBag.Medication_Id = new SelectList(medications, "medicationId", "description", medicalFollowUp.Medication_Id);

            return View(medicalFollowUp);
        }

        private async Task createReminders(Medication medication)
        {
            // Do nothing if no frequency or duration
            if (!medication.Frequency.HasValue || !medication.Duration.HasValue)
            {
                return;
            }
            
            // Remove existing reminders
            if (medication.Reminders.Count() != 0)
            {
                foreach (var reminder in medication.Reminders)
                {
                    await _notifications.DeleteNotification(reminder);
                }
            }

            DateTime now = DateTime.Now;
            DateTime enddate = now;
            TimeSpan interval = new TimeSpan(0);

            switch (medication.FrequencyValue.Value)
            {
                case RecurringType.Hours:
                    interval = new TimeSpan(medication.Frequency.Value, 0, 0);
                    break;
                case RecurringType.Days:
                    interval = new TimeSpan(medication.Frequency.Value, 0, 0, 0);
                    break;
                case RecurringType.Weeks:
                    interval = new TimeSpan((7 * medication.Frequency.Value), 0, 0, 0);
                    break;
                case RecurringType.Months:
                    interval = new TimeSpan((28 * medication.Frequency.Value), 0, 0, 0);
                    break;
            }

            switch (medication.DurationValue.Value)
            {
                case RecurringType.Hours:
                    enddate = now.AddHours(medication.Frequency.Value);
                    break;
                case RecurringType.Days:
                    enddate = now.AddDays(medication.Frequency.Value);
                    break;
                case RecurringType.Weeks:
                    enddate = now.AddDays(7 * medication.Frequency.Value);
                    break;
                case RecurringType.Months:
                    enddate = now.AddDays(28 * medication.Frequency.Value);
                    break;
            }

            for (DateTime i = now; i <= enddate; i = i.Add(interval))
            {
                await _notifications.CreateNotification(
                    new Notification()
                    {
                        Medication_Id = medication.Id,
                        NotificationDate = i,
                        Animal_Id = medication.Animal_Id,
                        Type = NotificationType.Medication
                    }
                );
            }
        }
    }
}
