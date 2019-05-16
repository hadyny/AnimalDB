using AnimalDB.Repo.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IRosterService
    {
        Task<IEnumerable<Roster>> GetRosters();

        Task CreateRoster(Roster roster);

        Task<Roster> GetRosterById(int id);

        Task UpdateRoster(Roster roster);

        Task DeleteRoster(Roster roster);

        Task<IEnumerable<Roster>> GetRostersByRoomId(int id);

        Task<bool> CheckIfThereIsARosterThisWeekend(DateTime date, int? id = null);
    }
}
