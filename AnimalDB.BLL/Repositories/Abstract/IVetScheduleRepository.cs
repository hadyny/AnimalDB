using AnimalDB.Repo.Entities;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IVetScheduleRepository : IRepository<VetSchedule>
    {
        bool Exists();
        VetSchedule GetVetSchedule();
    }
}
