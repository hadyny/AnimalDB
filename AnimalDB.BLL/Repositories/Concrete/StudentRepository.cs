using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class StudentRepository : UserRepository<Student>, IStudentRepository
    {
        public StudentRepository(AnimalDBContext context) : base(context)
        {
        }

        public override async Task Delete(Student student)
        {
            student.Investigators.Clear();

            var animals = Context.Animals.ToList();

            foreach (var animal in animals.Where(m => m.Researcher_Id == student.Id))
            {
                animal.Researcher_Id = null;
                Context.Entry(animal).State = EntityState.Modified;
            }

            foreach (var medication in Context.Medications.Where(m => m.WhoToNotify_Id == student.Id))
            {
                medication.WhoToNotify_Id = medication.WhoToNotify_Id = null;
                Context.Entry(medication).State = EntityState.Modified;
            }

            Context.Students.Remove(student);
        }

        public IEnumerable<AnimalUser> GetStudentsAndInvestigators()
        {
            List<AnimalUser> animalUsers = new List<AnimalUser>();
            
            foreach (var student in Context.Students.ToList())
            {
                animalUsers.Add(student);
            }

            foreach (var investigator in Context.Investigators.ToList())
            {
                animalUsers.Add(investigator);
            }

            return animalUsers.ToList();
        }


        public async Task AddInvestigatorToStudent(string studentId, string investigatorId)
        {
            var student = await Context.Students.FindAsync(studentId);
            var investigator = await Context.Investigators.FindAsync(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Add(investigator);
        }

        public async Task RemoveInvestigatorFromStudent(string studentId, string investigatorId)
        {
            var student = await Context.Students.FindAsync(studentId);
            var investigator = await Context.Investigators.FindAsync(investigatorId);
            if (student == null || investigator == null) return;
            student.Investigators.Remove(investigator);
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}

