using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
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

        private IAnimal _animals;
        private INote _notes;
        private ITechnician _technicians;

        public NoteController()
        {
            this._animals = new AnimalRepo();
            this._notes = new NoteRepo();
            this._technicians = new TechnicianRepo();
        }

        // GET: /Note/
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
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;

            return View(_notes.GetNoteByAnimalId(id.Value));
        }

        // GET: /Note/Create
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
            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.TechnicianNotified_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName");
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

            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return HttpNotFound();
            }

            note.Animal_Id = id.Value;
            note.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                await AlertTechnicians(await _technicians.GetTechnicianById(note.TechnicianNotified_Id), note);
                await _notes.CreateNote(note);
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.AnimalName = animal.UniqueAnimalId;
            ViewBag.AnimalId = animal.Id;
            ViewBag.TechnicianNotified_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName", note.TechnicianNotified_Id);
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
            Note note = await _notes.GetNoteById(id.Value);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.TechnicianNotified_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName", note.TechnicianNotified_Id);
            return View(note);
        }

        // POST: /Note/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Note note, int? id)
        {
            note.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                await AlertTechnicians(await _technicians.GetTechnicianById(note.TechnicianNotified_Id), note, true);
                await _notes.UpdateNote(note);
                return RedirectToAction("Index", new { id = note.Animal_Id });
            }
            ViewBag.TechnicianNotified_Id = new SelectList(_technicians.GetTechnicians(), "Id", "FullName", note.TechnicianNotified_Id);
            return View(note);
        }

        // GET: /Note/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = await _notes.GetNoteById(id.Value);
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
            Note note = await _notes.GetNoteById(id);
            await _notes.DeleteNote(note);
            return RedirectToAction("Index", new { id = note.Animal_Id });
        }

        private async Task AlertTechnicians(Technician tech, Note note, bool update = false)
        {
            if (tech == null) return;

            note.Animal = await _animals.GetAnimalById(note.Animal_Id);
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
