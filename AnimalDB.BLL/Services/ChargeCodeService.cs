using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class ChargeCodeService : IChargeCodeService
    {
        private readonly IRepository<ChargeCode> _chargeCodes;

        public ChargeCodeService(IRepository<ChargeCode> chargeCodes)
        {
            this._chargeCodes = chargeCodes;
        }

        public async Task CreateChargeCode(ChargeCode chargeCode)
        {
            _chargeCodes.Insert(chargeCode);
            await _chargeCodes.Save();
        }

        public async Task DeleteChargeCode(ChargeCode chargeCode)
        {
            await _chargeCodes.Delete(chargeCode);
            await _chargeCodes.Save();
        }

        public async Task<ChargeCode> GetChargeCodeById(int id)
        {
            return await _chargeCodes.GetById(id);
        }

        public async Task<IEnumerable<ChargeCode>> GetChargeCodes()
        {
            return await _chargeCodes.GetAll();
        }

        public async Task UpdateChargeCode(ChargeCode chargeCode)
        {
            _chargeCodes.Update(chargeCode);
            await _chargeCodes.Save();
        }
    }
}
