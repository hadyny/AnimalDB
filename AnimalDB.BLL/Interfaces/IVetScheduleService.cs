using AnimalDB.Repo.Entities;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IVetScheduleService
    {
        Task<VetSchedule> GetVetSchedule();

        Task CreateVetSchedule(VetSchedule vetSchedule);

        Task UpdateVetSchedule(VetSchedule vetSchedule);

        Task DeleteVetSchedule(VetSchedule vetSchedule);

        Task<bool> DoesVetScheduleExist();
    }
}
