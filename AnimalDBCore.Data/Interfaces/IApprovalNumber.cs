using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IApprovalNumber
    {
        IEnumerable<ApprovalNumber> GetApprovalNumbers();

        Task CreateApprovalNumber(ApprovalNumber approvalNumber);

        Task<ApprovalNumber> GetApprovalNumberById(int id);

        Task UpdateApprovalNumber(ApprovalNumber approvalNumber);

        Task DeleteApprovalNumber(ApprovalNumber approvalNumber);
    }
}
