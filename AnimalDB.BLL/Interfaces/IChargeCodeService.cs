using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IChargeCodeService
    {
        Task<IEnumerable<ChargeCode>> GetChargeCodes();

        Task CreateChargeCode(ChargeCode chargeCode);

        Task<ChargeCode> GetChargeCodeById(int id);

        Task UpdateChargeCode(ChargeCode chargeCode);

        Task DeleteChargeCode(ChargeCode chargeCode);
    }
}
