using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IVetSchedule
    {
        IEnumerable<VetSchedule> GetVetSchedules();

        Task CreateVetSchedule(VetSchedule vetSchedule);

        Task<VetSchedule> GetVetScheduleById(int id);

        Task UpdateVetSchedule(VetSchedule vetSchedule);

        Task DeleteVetSchedule(VetSchedule vetSchedule);
    }
}
