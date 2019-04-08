using   AnimalDBCore.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IGroup
    {
        IEnumerable<Group> GetGroups();

        Task CreateGroup(Group group);

        Task<Group> GetGroupById(int id);

        Task UpdateGroup(Group group);

        Task DeleteGroup(Group group);

        IEnumerable<Group> GetGroupsByFeedingGroupId(int feedingGroupId);

        Task RemoveAnimalFromGroup(int animalId, int groupId);
    }
}
