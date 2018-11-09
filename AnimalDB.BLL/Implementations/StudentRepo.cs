using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AnimalDB.Repo.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
using Microsoft.Owin.Security;

namespace AnimalDB.Repo.Implementations
{
    public class StudentRepo : IStudent, IDisposable
    {

        private AnimalDBContext db;

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
            usermanager.AddToRole(student.Id, "Student");
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

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}

