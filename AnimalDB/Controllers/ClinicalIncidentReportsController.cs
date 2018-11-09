using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Student, Investigator, Veterinarian, Technician, Administrator")]
    public class ClinicalIncidentReportsController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IAnimal _animals;
        private IClinicalIncidentReport _clinicalIncidentReports;
        private IMedicationType _medicationTypes;
        private IVeterinarian _veterinarians;

        public ClinicalIncidentReportsController()
        {
            this._animals = new AnimalRepo();
            this._clinicalIncidentReports = new ClinicalIncidentReportRepo();
            this._medicationTypes = new MedicationTypeRepo();
            this._veterinarians = new VeterinarianRepo();
        }

        // GET: ClinicalIncidentReports
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }

            ViewBag.AnimalId = id.Value;
            ViewBag.AnimalName = animal.UniqueAnimalId;
            return View(_clinicalIncidentReports.GetClinicalIncidentReports().OrderByDescending(m => m.Timestamp));
        }

        // GET: ClinicalIncidentReports/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            

            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.CageLocation = await _animals.GetAnimalsCageLocationDescription(animal.Id);
            
            var model = new ClinicalIncidentReport
            {
                Animal = animal,
                Animal_Id = animal.Id
            };

            model.EthicsNumber = await _animals.GetAnimalsEthicsNumberDescription(animal.Id);
           
            return View(model);
        }

        // POST: ClinicalIncidentReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClinicalIncidentReport clinicalIncidentReport)
        {
            clinicalIncidentReport.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _clinicalIncidentReports.CreateClinicalIncidentReport(clinicalIncidentReport);

                SendEmailToVets(update: false, report: clinicalIncidentReport);
                return RedirectToAction("Index", new { id = clinicalIncidentReport.Animal_Id });
                
            }

            ViewBag.MedicationType_Id = new SelectList(_medicationTypes.GetMedicationTypes(), "Id", "Description");
            return View(clinicalIncidentReport);
        }

        // GET: ClinicalIncidentReports/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClinicalIncidentReport clinicalIncidentReport = await _clinicalIncidentReports.GetClinicalIncidentReportById(id.Value);
            if (clinicalIncidentReport == null)
            {
                return HttpNotFound();
            }
            
            var animal = await _animals.GetAnimalById(clinicalIncidentReport.Animal_Id);
            ViewBag.CageLocation = await _animals.GetAnimalsCageLocationDescription(animal.Id);
            
            clinicalIncidentReport.Animal = animal;
            clinicalIncidentReport.EthicsNumber = await _animals.GetAnimalsEthicsNumberDescription(animal.Id);
            return View(clinicalIncidentReport);
        }

        // POST: ClinicalIncidentReports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClinicalIncidentReport clinicalIncidentReport)
        {
            
            if (ModelState.IsValid)
            {
                await _clinicalIncidentReports.UpdateClinicalIncidentReport(clinicalIncidentReport);
                SendEmailToVets(update: false, report: clinicalIncidentReport);
                return RedirectToAction("Index", new { id = clinicalIncidentReport.Animal_Id });
                
            }
            return View(clinicalIncidentReport);
        }

        // GET: ClinicalIncidentReports/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var clinicalIncidentReport = await _clinicalIncidentReports.GetClinicalIncidentReportById(id.Value);
            if (clinicalIncidentReport == null)
            {
                return HttpNotFound();
            }
            return View(clinicalIncidentReport);
        }

        // POST: ClinicalIncidentReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var clinicalIncidentReport = await _clinicalIncidentReports.GetClinicalIncidentReportById(id);
            await _clinicalIncidentReports.DeleteClinicalIncidentReport(clinicalIncidentReport);
            return RedirectToAction("Index", new { id = clinicalIncidentReport.Animal_Id });
        }

        public void SendEmailToVets(bool update, ClinicalIncidentReport report)
        {
            MailMessage msg = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["SystemEmail"])
            };
            string body = "";
            if (update)
            {
                msg.Subject = "Clinical Incident Report Updated";
                body = "The Clinical Incident Report for the animal " + report.Animal.UniqueAnimalId + " has been updated.\r\n\r\n";
            }
            else
            {
                msg.Subject = "New Clinical Incident Report created for animal";
                body = "A Clinical Incident Report has been created for the animal " + report.Animal.UniqueAnimalId + ".\r\n\r\n";
            }

            body += "Go here to view the report: https://web.psy.otago.ac.nz/adb/ClinicalIncidentReports/Edit/" + report.Id;

            msg.Body = body;
            foreach (var vet in _veterinarians.GetVeterinarians().Where(m => m.Email != ""))
            {
                msg.To.Add(vet.Email);
            }

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SystemEmail"]);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SystemUsername"], ConfigurationManager.AppSettings["SystemPassword"]);
            smtp.Port = 587;

            if (msg.To.Count() != 0)
            {
                smtp.Send(msg);
            }
        }
    }
}
