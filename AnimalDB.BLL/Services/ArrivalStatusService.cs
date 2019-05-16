using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class ArrivalStatusService : IArrivalStatusService
    {
        //private readonly AnimalDBContext db;
        private readonly IRepository<ArrivalStatus> _arrivalStatus;

        public ArrivalStatusService(IRepository<ArrivalStatus> arrivalStatus)
        {
            this._arrivalStatus = arrivalStatus;
        }

        public async Task CreateArrivalStatus(ArrivalStatus arrivalStatus)
        {
            _arrivalStatus.Insert(arrivalStatus);
            await _arrivalStatus.Save();
        }

        public async Task DeleteArrivalStatus(ArrivalStatus arrivalStatus)
        {
            await _arrivalStatus.Delete(arrivalStatus);
            await _arrivalStatus.Save();
        }

        public async Task<IEnumerable<ArrivalStatus>> GetArrivalStatus()
        {
            return await _arrivalStatus.GetAll();
        }

        public async Task<ArrivalStatus> GetArrivalStatusById(int id)
        {
            return await _arrivalStatus.GetById(id);
        }

        public async Task UpdateArrivalStatus(ArrivalStatus arrivalStatus)
        {
            _arrivalStatus.Update(arrivalStatus);
            await _arrivalStatus.Save();
        }
    }
}
