using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();

        private readonly IUnitOfWork _unitOfWork;

        public NoteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Note/
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
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;

            return View(_unitOfWork.Notes.GetByAnimalId(id.Value));
        }

        // GET: /Note/Create
        public async Task<ActionResult> Create(int? id)
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
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.TechnicianNotified_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName");
            return View();
        }

        // POST: /Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Note note, int? id)
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

            note.Animal_Id = id.Value;
            note.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                await AlertTechnicians(await _unitOfWork.Technicians.GetById(note.TechnicianNotified_Id), note);
                _unitOfWork.Notes.Insert(note);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id });
            }

            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.TechnicianNotified_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName", note.TechnicianNotified_Id);
            note.Type = null;
            return View(note);
        }

        // GET: /Note/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = await _unitOfWork.Notes.GetById(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.TechnicianNotified_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName", note.TechnicianNotified_Id);
            return View(note);
        }

        // POST: /Note/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Note note)
        {
            note.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                await AlertTechnicians(await _unitOfWork.Technicians.GetById(note.TechnicianNotified_Id), note, true);
                _unitOfWork.Notes.Update(note);
                await _unitOfWork.Complete();
                return RedirectToAction("Index", new { id = note.Animal_Id });
            }
            ViewBag.TechnicianNotified_Id = new SelectList(await _unitOfWork.Technicians.Get(), "Id", "FullName", note.TechnicianNotified_Id);
            return View(note);
        }

        // GET: /Note/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = await _unitOfWork.Notes.GetById(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: /Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Note note = await _unitOfWork.Notes.GetById(id);
            _unitOfWork.Notes.Delete(note);
            await _unitOfWork.Complete();
            return RedirectToAction("Index", new { id = note.Animal_Id });
        }

        private async Task AlertTechnicians(Technician tech, Note note, bool update = false)
        {
            if (tech == null) return;

            note.Animal = await _unitOfWork.Animals.GetById(note.Animal_Id);
            MailMessage msg = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["SystemEmail"])
            };
            
            StringBuilder body = new StringBuilder();
            if (update)
            {
                msg.Subject = "Note updated for animal";
                body.Append("A note has been updated for the animal " + note.Animal.UniqueAnimalId + ":\r\n\r\n");
            }
            else
            {
                msg.Subject = "New note added to animal";
                body.Append("A note has been added to the animal " + note.Animal.UniqueAnimalId + ":\r\n\r\n");
            }
            
            body.Append("Note type: " + note.Type.ToString() + "\r\n\r\n");
            body.Append(note.Text);
            msg.Body = body.ToString();
            msg.To.Add(tech.Email);

            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"])
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SystemUsername"], ConfigurationManager.AppSettings["SystemPassword"]),
                Port = 587
            };

            if (msg.To.Count() != 0) {
                smtp.Send(msg);
            }
        }
    }
}
