using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IApprovalNumberService
    {
        Task<IEnumerable<ApprovalNumber>> GetApprovalNumbers();

        Task CreateApprovalNumber(ApprovalNumber approvalNumber);

        Task<ApprovalNumber> GetApprovalNumberById(int id);

        Task UpdateApprovalNumber(ApprovalNumber approvalNumber);

        Task DeleteApprovalNumber(ApprovalNumber approvalNumber);
    }
}
