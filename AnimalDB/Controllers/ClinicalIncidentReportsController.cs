using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
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
        private readonly IUnitOfWork _unitOfWork;

        public ClinicalIncidentReportsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: ClinicalIncidentReports
        public async Task<ActionResult> Index(int? id)
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

            ViewBag.AnimalId = id.Value;
            ViewBag.AnimalName = animal.UniqueAnimalId;
            var reports = await _unitOfWork.ClinicalIncidentReports.Get(m => m.Animal_Id == id.Value);
            return View(reports.OrderByDescending(m => m.Timestamp));
        }

        // GET: ClinicalIncidentReports/Create
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
            
            ViewBag.CageLocation = _unitOfWork.Animals.GetAnimalsCageLocationDescription(animal.Id);
            
            var model = new ClinicalIncidentReport
            {
                Animal = animal,
                Animal_Id = animal.Id
            };

            model.EthicsNumber = _unitOfWork.Animals.GetAnimalsEthicsNumberDescription(animal.Id);
           
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
                _unitOfWork.ClinicalIncidentReports.Insert(clinicalIncidentReport);
                await _unitOfWork.Complete();

                await SendEmailToVets(update: false, report: clinicalIncidentReport);
                return RedirectToAction("Index", new { id = clinicalIncidentReport.Animal_Id });
                
            }

            ViewBag.MedicationType_Id = new SelectList(await _unitOfWork.MedicationTypes.Get(), "Id", "Description");
            return View(clinicalIncidentReport);
        }

        // GET: ClinicalIncidentReports/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClinicalIncidentReport clinicalIncidentReport = await _unitOfWork.ClinicalIncidentReports.GetById(id.Value);
            if (clinicalIncidentReport == null)
            {
                return HttpNotFound();
            }
            
            var animal = await _unitOfWork.Animals.GetById(clinicalIncidentReport.Animal_Id);
            ViewBag.CageLocation = _unitOfWork.Animals.GetAnimalsCageLocationDescription(animal.Id);
            
            clinicalIncidentReport.Animal = animal;
            clinicalIncidentReport.EthicsNumber = _unitOfWork.Animals.GetAnimalsEthicsNumberDescription(animal.Id);
            return View(clinicalIncidentReport);
        }

        // POST: ClinicalIncidentReports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClinicalIncidentReport clinicalIncidentReport)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.ClinicalIncidentReports.Update(clinicalIncidentReport);
                await _unitOfWork.Complete();
                await SendEmailToVets(update: false, report: clinicalIncidentReport);
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
            var clinicalIncidentReport = await _unitOfWork.ClinicalIncidentReports.GetById(id.Value);
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
            var clinicalIncidentReport = await _unitOfWork.ClinicalIncidentReports.GetById(id);
            _unitOfWork.ClinicalIncidentReports.Delete(clinicalIncidentReport);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id = clinicalIncidentReport.Animal_Id });
        }

        public async Task SendEmailToVets(bool update, ClinicalIncidentReport report)
        {
            var animal = await _unitOfWork.Animals.GetById(report.Animal_Id);
            MailMessage msg = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["SystemEmail"])
            };
            string body = "";
            if (update)
            {
                msg.Subject = "Clinical Incident Report Updated";
                body = "The Clinical Incident Report for the animal " + animal.UniqueAnimalId + " has been updated.\r\n\r\n";
            }
            else
            {
                msg.Subject = "New Clinical Incident Report created for animal";
                body = "A Clinical Incident Report has been created for the animal " + animal.UniqueAnimalId + ".\r\n\r\n";
            }

            body += "Go here to view the report: https://web.psy.otago.ac.nz/adb/ClinicalIncidentReports/Edit/" + report.Id;

            msg.Body = body;
            var vets = await _unitOfWork.Veterinarians.Get();
            foreach (var vet in vets.Where(m => m.Email != ""))
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
