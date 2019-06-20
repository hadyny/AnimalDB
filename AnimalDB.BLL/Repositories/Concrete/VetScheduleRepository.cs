using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Repositories.Abstract;
using AnimalDB.Repositories.Concrete;
using System.Linq;

namespace AnimalDB.Repo.Repositories.Concrete
{
    public class VetScheduleRepository : Repository<VetSchedule>, IVetScheduleRepository
    {
        public VetScheduleRepository(AnimalDBContext context) : base(context)
        {
        }

        public bool Exists()
        {
            return Context.VetSchedules.Count() != 0;
        }

        public VetSchedule GetVetSchedule()
        {
            return Context.VetSchedules.FirstOrDefault();
        }

        public AnimalDBContext Context
        {
            get { return base.db; }
        }
    }
}
