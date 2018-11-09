using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IArrivalStatus
    {
        IEnumerable<ArrivalStatus> GetArrivalStatus();

        Task CreateArrivalStatus(ArrivalStatus arrivalStatus);

        Task<ArrivalStatus> GetArrivalStatusById(int id);

        Task UpdateArrivalStatus(ArrivalStatus arrivalStatus);

        Task DeleteArrivalStatus(ArrivalStatus arrivalStatus);
    }
}
