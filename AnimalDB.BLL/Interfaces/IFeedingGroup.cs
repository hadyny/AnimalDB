using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IFeedingGroup
    {
        IEnumerable<FeedingGroup> GetFeedingGroups();

        Task CreateFeedingGroup(FeedingGroup feedingGroup);

        Task<FeedingGroup> GetFeedingGroupById(int id);

        Task UpdateFeedingGroup(FeedingGroup feedingGroup);

        Task DeleteFeedingGroup(FeedingGroup feedingGroup);

        IEnumerable<FeedingGroup> GetFeedingGroupsByRoomId(int roomId);

        Task UpdateFeedingGroupPage(FeedingGroup model);

        ICollection<Animal> GetAnimalsInFeedingGroup(int feedingGroupId);

        Task RemoveAnimalFromFeedingGroup(int animalId);

        int? GetFeedingGroupByAnimalId(int animalId);
    }
}
