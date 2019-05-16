using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class VetScheduleService : IVetScheduleService
    {
        private readonly IRepository<VetSchedule> _vetSchedules;

        public VetScheduleService(IRepository<VetSchedule> vetSchedules)
        {
            this._vetSchedules = vetSchedules;
        }

        public async Task CreateVetSchedule(VetSchedule vetSchedule)
        {
            _vetSchedules.Insert(vetSchedule);
            await _vetSchedules.Save();
        }

        public async Task DeleteVetSchedule(VetSchedule vetSchedule)
        {
            await _vetSchedules.Delete(vetSchedule);
            await _vetSchedules.Save();
        }

        public async Task<bool> DoesVetScheduleExist()
        {
            var schedules = await _vetSchedules.GetAll();
            return schedules.Count() != 0;
        }

        public async Task<VetSchedule> GetVetSchedule()
        {
            var schedules = await _vetSchedules.GetAll();
            if (!await DoesVetScheduleExist())
            {
                return null;
            }
            else
            {
                return schedules.First();
            }
        }

        public async Task UpdateVetSchedule(VetSchedule vetSchedule)
        {
            _vetSchedules.Update(vetSchedule);
            await _vetSchedules.Save();
        }
    }
}
