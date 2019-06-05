using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Repo.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUserRepository<Student> _students;
        private readonly IUserRepository<Investigator> _investigators;
        private readonly IRepository<Animal> _animals;
        private readonly IRepository<Medication> _medications;

        public StudentService(IUserRepository<Student> students,
                              IUserRepository<Investigator> investigators,
                              IRepository<Animal> animals,
                              IRepository<Medication> medications)
        {
            this._students = students;
            this._investigators = investigators;
            this._animals = animals;
            this._medications = medications;
        }

        public async Task CreateStudent(Student student)
        {
            await _students.Insert(student, Repo.Enums.UserType.Student);
        }

        public async Task DeleteStudent(Student student)
        {
            foreach (var inv in student.Investigators.ToList())
            {
                student.Investigators.Remove(inv);
            }
            foreach (var animal in await _animals.GetAll(m => m.Researcher_Id == student.Id))
            {
                animal.Researcher_Id = null;
                _animals.Update(animal);
            }
            foreach (var medication in await _medications.GetAll(m => m.WhoToNotify_Id == student.Id))
            {
                medication.WhoToNotify_Id = medication.WhoToNotify_Id = null;
                _medications.Update(medication);
            }

            await _animals.Save();
            await _medications.Save();
            await _students.Save();

            await _students.Delete(student);
        }

        public async Task<Student> GetStudentByUsername(string username)
        {
            return await _students.GetByUsername(username);
        }

        public async Task<Student> GetStudentById(string id)
        {
            return await _students.GetById(id);
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _students.GetAll();
        }

        public async Task<IEnumerable<AnimalUser>> GetStudentsAndInvestigators()
        {
            List<AnimalUser> animalUsers = new List<AnimalUser>();

            foreach (var student in await _students.GetAll())
            {
                animalUsers.Add(student);
            }

            foreach (var investigator in await _investigators.GetAll())
            {
                animalUsers.Add(investigator);
            }

            return animalUsers;
        }

        public async Task UpdateStudent(Student student)
        {
            _students.Update(student);
            await _students.Save();
        }

        public async Task SetAuthCookie(string userName)
        {
            using (var db = new AnimalDBContext())
            {
                Student user = await GetStudentByUsername(userName);
                var AdminManager = new UserManager<Student>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Student>(db));
                var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
            }
        }

        public async Task AddInvestigatorToStudent(string studentId, string investigatorId)
        {
            /*
            var student = await _students.GetById(studentId);
            var investigator = await _investigators.GetById(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Add(investigator);
            await _students.Save();*/

            using (var db = new AnimalDBContext())
            {
                var student = await db.Students.FindAsync(studentId);
                var investigator = await db.Investigators.FindAsync(investigatorId);
                if (student == null || investigator == null) return;
                student.Investigators.Add(investigator);
                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveInvestigatorFromStudent(string studentId, string investigatorId)
        {
            /*var student = await _students.GetById(studentId);
            var investigator = await _investigators.GetById(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Remove(investigator);
            await _students.Save();*/

            using (var db = new AnimalDBContext())
            {
                var student = await db.Students.FindAsync(studentId);
                var investigator = await db.Investigators.FindAsync(investigatorId);
                if (student == null || investigator == null) return;
                student.Investigators.Remove(investigator);
                await db.SaveChangesAsync();
            }
        }
    }
}

