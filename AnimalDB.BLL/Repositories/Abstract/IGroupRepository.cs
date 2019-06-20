using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Repositories.Abstract
{
    public interface IGroupRepository : IRepository<Group>
    {
        IEnumerable<Group> GetByFeedingGroupId(int feedingGroupId);
        Task RemoveAnimalFromGroup(int animalId, int groupId);
    }
}
