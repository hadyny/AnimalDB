using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class GroupService : IGroupService
    {
        private readonly IRepository<Group> _groups;
        private readonly IRepository<Animal> _animals;

        public GroupService(IRepository<Group> groups, IRepository<Animal> animals)
        {
            this._groups = groups;
            this._animals = animals;
        }

        public async Task CreateGroup(Group group)
        {
            _groups.Insert(group);
            await _groups.Save();
        }

        public async Task DeleteGroup(Group group)
        {
            foreach (var animal in await _animals.GetAll(m => m.Group_Id == group.Id))
            {
                animal.Group_Id = null;
            }

            await _groups.Delete(group);
            await _animals.Save();
            await _groups.Save();
        }

        public async Task<Group> GetGroupById(int id)
        {
            return await _groups.GetById(id);
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            return await _groups.GetAll();
        }

        public async Task UpdateGroup(Group group)
        {
            _groups.Update(group);
            await _groups.Save();
        }

        public async Task<IEnumerable<Group>> GetGroupsByFeedingGroupId(int feedingGroupId)
        {
            return await _groups.GetAll(m => m.FeedingGroup_Id == feedingGroupId);
        }

        public async Task RemoveAnimalFromGroup(int animalId, int groupId)
        {
            var animal = await _animals.GetById(animalId);
            var group = await _groups.GetById(groupId);
            group.Animals.Remove(animal);
            _groups.Update(group);
            await _groups.Save();
        }
    }
}
