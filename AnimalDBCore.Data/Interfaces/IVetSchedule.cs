using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
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
