using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class ApprovalNumberService : IApprovalNumberService
    {
        private readonly IRepository<ApprovalNumber> _approvalNumbers;

        public ApprovalNumberService(IRepository<ApprovalNumber> approvalNumbers)
        {
            this._approvalNumbers = approvalNumbers;
        }

        public async Task CreateApprovalNumber(ApprovalNumber approvalNumber)
        {
            _approvalNumbers.Insert(approvalNumber);
            await _approvalNumbers.Save();
        }

        public async Task DeleteApprovalNumber(ApprovalNumber approvalNumber)
        {
           
            await _approvalNumbers.Delete(approvalNumber.Id);
            await _approvalNumbers.Save();
        }

        public async Task<ApprovalNumber> GetApprovalNumberById(int id)
        {
            return await _approvalNumbers.GetById(id);
        }

        public async Task<IEnumerable<ApprovalNumber>> GetApprovalNumbers()
        {
            return await _approvalNumbers.GetAll();
        }

        public async Task UpdateApprovalNumber(ApprovalNumber approvalNumber)
        {
            _approvalNumbers.Update(approvalNumber);
            await _approvalNumbers.Save();
        }
    }
}
