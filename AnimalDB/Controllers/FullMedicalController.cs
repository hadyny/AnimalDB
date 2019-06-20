using AnimalDB.Repo.Services;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AnimalDB.Repo.Contexts;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class FullMedicalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FullMedicalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //
        // GET: /FullMedical/
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var animal = await _unitOfWork.Animals.GetById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }

            IEnumerable<Models.MedicalReportItem> items;

            var notes = _unitOfWork.Notes.GetByAnimalId(animal.Id);
            items = notes
                        .Select(m => new Models.MedicalReportItem()
                        {
                                Description = m.Text,
                                Details = m.Type.ToString(),
                                Type = "Note",
                                Timestamp = m.Timestamp,
                                Css = "note"
                        });

            var followUps = _unitOfWork.MedicationFollowUps.GetByAnimalId(animal.Id);
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

            var surgicalNotes = _unitOfWork.SurgicalNotes.GetByAnimalId(animal.Id);
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
