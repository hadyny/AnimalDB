using AnimalDB.Repo.Services;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class FullMedicalController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly IAnimalService _animals;
        private readonly INoteService _notes;
        private readonly IMedicationFollowUpService _medicationFollowUps;
        private readonly ISurgicalNoteService _surgicalNotes;

        public FullMedicalController(IAnimalService animals, 
                                     INoteService notes, 
                                     IMedicationFollowUpService medicationFollowUps, 
                                     ISurgicalNoteService surgicalNotes)
        {
            this._animals = animals;
            this._notes = notes;
            this._medicationFollowUps = medicationFollowUps;
            this._surgicalNotes = surgicalNotes;

        }
        //
        // GET: /FullMedical/
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

            IEnumerable<Models.MedicalReportItem> items;

            var notes = await _notes.GetNotesByAnimalId(animal.Id);
            items = notes
                        .Select(m => new Models.MedicalReportItem()
                        {
                                Description = m.Text,
                                Details = m.Type.ToString(),
                                Type = "Note",
                                Timestamp = m.Timestamp,
                                Css = "note"
                        });

            var followUps = await _medicationFollowUps.GetMedicationFollowsUpByAnimalId(animal.Id);
            items = items
                        .Concat(followUps
                        .Select(m => new Models.MedicalReportItem()
                        {
                            Description = m.Medication.Dosage,
                            Details = m.Medication.MedicationType.Description,
                            Type = "Medication",
                            Timestamp = m.Timestamp,
                            Css = "med"
                        }));

            var surgicalNotes = await _surgicalNotes.GetSurgicalNotesByAnimalId(animal.Id);
            items = items
                        .Concat(surgicalNotes
                        .Select(m => new Models.MedicalReportItem()
                        {
                            Description = m.SurgeryType.Description,
                            Type = " Surgical Note",
                            Timestamp = m.Timestamp,
                            Css = "surg"
                        }));

            var model = new Models.MedicalReportViewModel()
            {
                Animal = animal,
                MedicalItems = items
            };

            return View(model);
        }       
    }
}
