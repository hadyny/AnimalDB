using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AnimalDB.Repo.Implementations
{
    public class StudentRepo : IStudent
    {

        private readonly AnimalDBContext db;

        public StudentRepo()
        {
            this.db = new AnimalDBContext();
        }

        public StudentRepo(AnimalDBContext db)
        {
            this.db = db;
        }

        public async Task CreateStudent(Student student)
        {
            var usermanager = new UserManager<Student>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Student>(db));
            var result = await usermanager.CreateAsync(student, "Password not required");
            if (result.Succeeded) usermanager.AddToRole(student.Id, "Student");
        }

        public async Task DeleteStudent(Student student)
        {
            foreach (var inv in student.Investigators.ToList())
            {
                student.Investigators.Remove(inv);
            }
            foreach (var animal in db.Animals.Where(m => m.Researcher_Id == student.Id))
            {
                animal.Researcher_Id = null;
            }
            foreach (var medication in db.Medications.Where(m => m.WhoToNotify_Id == student.Id))
            {
                medication.WhoToNotify_Id = medication.WhoToNotify_Id = null;
            }
            await db.SaveChangesAsync();
            var usermanager = new UserManager<Student>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Student>(db));
            usermanager.RemoveFromRole(student.Id, "Student");
            await usermanager.DeleteAsync(student);
        }

        public Student GetStudentByUsername(string username)
        {
            return db.Students.SingleOrDefault(m => m.UserName == username);
        }

        public async Task<Student> GetStudentById(string id)
        {
            return await db.Students.FindAsync(id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return db.Students.ToList();
        }

        public IEnumerable<AnimalUser> GetStudentsAndInvestigators()
        {
            List<AnimalUser> animalUsers = new List<AnimalUser>();

            foreach (var student in db.Students)
            {
                animalUsers.Add(student);
            }

            foreach (var investigator in db.Investigators)
            {
                animalUsers.Add(investigator);
            }

            return animalUsers;
        }

        public async Task UpdateStudent(Student student)
        {
            db.Entry(student).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Student user = GetStudentByUsername(userName);
            var AdminManager = new UserManager<Student>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<Student>(db));
            var adminIdentity = await AdminManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, adminIdentity);
        }

        public async Task AddInvestigatorToStudent(string studentId, string investigatorId)
        {
            var student = await db.Students.FindAsync(studentId);
            var investigator = await db.Investigators.FindAsync(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Add(investigator);
            await db.SaveChangesAsync();
        }

        public async Task RemoveInvestigatorFromStudent(string studentId, string investigatorId)
        {
            var student = await db.Students.FindAsync(studentId);
            var investigator = await db.Investigators.FindAsync(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Remove(investigator);
            await db.SaveChangesAsync();
        }
    }
}

