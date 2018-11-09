using AnimalDB.Repo.Implementations;
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
        private IAnimal _animals;
        private INote _notes;
        private IMedicationFollowUp _medicationFollowUps;
        private ISurgicalNote _sugicalNotes;

        public FullMedicalController()
        {
            this._animals = new AnimalRepo();
            this._notes = new NoteRepo();
            this._medicationFollowUps = new MedicationFollowUpRepo();
            this._sugicalNotes = new SurgicalNoteRepo();

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

            items = _notes.GetNoteByAnimalId(animal.Id)
                        .Select(m => new Models.MedicalReportItem()
                        {
                                Description = m.Text,
                                Details = m.Type.ToString(),
                                Type = "Note",
                                Timestamp = m.Timestamp,
                                Css = "note"
                        });
            
            items = items
                        .Concat(_medicationFollowUps.GetMedicationFollowUpByAnimalId(animal.Id)
                        .Select(m => new Models.MedicalReportItem()
                        {
                            Description = m.Medication.Dosage,
                            Details = m.Medication.MedicationType.Description,
                            Type = "Medication",
                            Timestamp = m.Timestamp,
                            Css = "med"
                        }));

            items = items
                        .Concat(_sugicalNotes.GetSurgicalNoteByAnimalId(animal.Id)
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
