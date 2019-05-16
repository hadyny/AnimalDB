using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IFeedingGroupService
    {
        Task<IEnumerable<FeedingGroup>> GetFeedingGroups();

        Task CreateFeedingGroup(FeedingGroup feedingGroup);

        Task<FeedingGroup> GetFeedingGroupById(int id);

        Task UpdateFeedingGroup(FeedingGroup feedingGroup);

        Task DeleteFeedingGroup(FeedingGroup feedingGroup);

        Task<IEnumerable<FeedingGroup>> GetFeedingGroupsByRoomId(int roomId);

        Task UpdateFeedingGroupPage(FeedingGroup model);

        Task<IEnumerable<Animal>> GetAnimalsInFeedingGroup(int feedingGroupId);

        Task RemoveAnimalFromFeedingGroup(int animalId);

        Task<int?> GetFeedingGroupByAnimalId(int animalId);
    }
}
