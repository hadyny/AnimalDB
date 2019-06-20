using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class NoteRepository : Repository<Note>, INoteRepository
    {
        public NoteRepository(AnimalDBContext context) : base(context)
        {
        }

        public IEnumerable<Note> GetByAnimalId(int animalId)
        {
            return Context.Notes.Where(m => m.Animal_Id == animalId).ToList();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
