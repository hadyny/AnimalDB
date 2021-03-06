﻿using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<Group>> GetGroups();

        Task CreateGroup(Group group);

        Task<Group> GetGroupById(int id);

        Task UpdateGroup(Group group);

        Task DeleteGroup(Group group);

        Task<IEnumerable<Group>> GetGroupsByFeedingGroupId(int feedingGroupId);

        Task RemoveAnimalFromGroup(int animalId, int groupId);
    }
}
