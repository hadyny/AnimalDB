using AnimalDB.Functions;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator,Technician")]
    public class StudentsController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();

        private IStudent _students;
        private IInvestigator _investigators;

        public StudentsController()
        {
            _students = new StudentRepo();
            _investigators = new InvestigatorRepo();
        }

        // GET: Students
        public ActionResult Index()
        {
            return View(_students.GetStudents());
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.Investigator_Id = new SelectList(_investigators.GetInvestigators(), "Id", "FullName");
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Investigator_Id,UserName,Email,FirstName,LastName")] Student student)
        {
            string error = await UserManagement.CreateAnimalUser(student, Repo.Enums.UserType.Student);

            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", error);
            }

            return View(student);
        }

        // GET: Students/AddInvestigator/5
        public async Task<ActionResult> AddInvestigator(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _students.GetStudentById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var model = new Models.AddInvestigatorVM
            {
                StudentId = student.Id,
                Investigators = student.Investigators
            };
            ViewBag.StudentName = student.FullName;
            List<Investigator> investigators = _investigators.GetInvestigators().Where(m => !m.Students.Contains(student)).ToList();
            ViewBag.Selected_Investigator_Id = new SelectList(investigators, "Id", "FullName");

            return View(model);
        }

        // POST: Students/AddInvestigator/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddInvestigator(Models.AddInvestigatorVM model)
        {
            // Just this needed to be implemented, and removing functions then update database and test creating students, adding
            // investigators, removing investigators, and student access
            Student student = await _students.GetStudentById(model.StudentId);

            if (ModelState.IsValid)
            {
                await _students.AddInvestigatorToStudent(studentId: model.StudentId, investigatorId: model.Selected_Investigator_Id);
                //student.Investigators.Add(await _investigators.GetInvestigatorById(model.Selected_Investigator_Id));
                //await _students.UpdateStudent(student);
            }

            ViewBag.StudentName = student.FullName;
            ViewBag.Selected_Investigator_Id = new SelectList(_investigators.GetInvestigators().Where(m => !m.Students.Contains(student)), "Id", "FullName");
            model.Investigators = student.Investigators;
            return View(model);
        }

        // GET: Students/RemoveInvestigator/5?inv=6
        public async Task<ActionResult> RemoveInvestigator(string id, string inv)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(inv))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var student = await _students.GetStudentById(id);

            var investigtor = await _investigators.GetInvestigatorById(inv);

            if (student == null || investigtor == null)
            {
                return HttpNotFound();
            }
            
            await _students.RemoveInvestigatorFromStudent(studentId: student.Id, investigatorId: investigtor.Id);

            return RedirectToAction("AddInvestigator", new { id });
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _students.GetStudentById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Student student = await _students.GetStudentById(id);
            await _students.DeleteStudent(student);
            return RedirectToAction("Index");
        }
    }
}
