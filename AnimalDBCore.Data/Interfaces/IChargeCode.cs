using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IChargeCode
    {
        IEnumerable<ChargeCode> GetChargeCodes();

        Task CreateChargeCode(ChargeCode chargeCode);

        Task<ChargeCode> GetChargeCodeById(int id);

        Task UpdateChargeCode(ChargeCode chargeCode);

        Task DeleteChargeCode(ChargeCode chargeCode);
    }
}
