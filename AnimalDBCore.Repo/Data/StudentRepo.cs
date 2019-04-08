using AnimalDBCore.Infrastructure.Contexts;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AnimalDBCore.Core.Interfaces;

namespace AnimalDBCore.Infrastructure.Data
{
    public class StudentRepo : IStudent<Student>, IDisposable
    {
        private readonly AnimalDBContext _db;
        private readonly SignInManager<Student> _signInManager;
        private readonly UserManager<Student> _userManager;

        public StudentRepo(AnimalDBContext db, SignInManager<Student> signinManager, UserManager<Student> userManager)
        {
            _db = db;
            _signInManager = signinManager;
            _userManager = userManager;
        }

        public async Task CreateStudent(Student student)
        {
            var result = await _userManager.CreateAsync(student, "Password not required");
            if (!result.Succeeded)
            {
                return;
            }
            await _userManager.AddToRoleAsync(student, "Student");
        }

        public async Task DeleteStudent(Student student)
        {
            student.Investigators.Clear();
            
            foreach (var animal in _db.Animals.Where(m => m.Researcher_Id == student.Id))
            {
                animal.Researcher_Id = null;
            }
            foreach (var medication in _db.Medications.Where(m => m.WhoToNotify_Id == student.Id))
            {
                medication.WhoToNotify_Id = medication.WhoToNotify_Id = null;
            }
            await _db.SaveChangesAsync();
            await _userManager.RemoveFromRoleAsync(student, "Student");
            await _userManager.DeleteAsync(student);
            await _db.SaveChangesAsync();
        }

        public Student GetStudentByUsername(string username)
        {
            return _db.Students.SingleOrDefault(m => m.UserName == username);
        }

        public async Task<Student> GetStudentById(string id)
        {
            return await _db.Students.FindAsync(id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _db.Students.ToList();
        }

        public async Task UpdateStudent(Student student)
        {
            _db.Entry(student).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task SetAuthCookie(string userName)
        {
            Student user = GetStudentByUsername(userName);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task AddInvestigatorToStudent(string studentId, string investigatorId)
        {
            var student = await _db.Students.FindAsync(studentId);
            var investigator = await _db.Investigators.FindAsync(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Add(investigator);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveInvestigatorFromStudent(string studentId, string investigatorId)
        {
            var student = await _db.Students.FindAsync(studentId);
            var investigator = await _db.Investigators.FindAsync(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Remove(investigator);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            ((IDisposable)_db).Dispose();
        }
    }
}

