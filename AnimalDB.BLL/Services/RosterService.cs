using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class RosterService : IRosterService
    {
        private readonly IRepository<Roster> _rosters;

        public RosterService(IRepository<Roster> rosters)
        {
            this._rosters = rosters;
        }

        public async Task<bool> CheckIfThereIsARosterThisWeekend(DateTime date, int? id = null)
        {
            var rosters = await _rosters.GetAll(m => m.Date == date);
            if (id == null)
            {
                return rosters.Count() != 0;
            }
            else
            {
                return rosters.Count(m => m.Id != id) != 0;
            }
        }

        public async Task CreateRoster(Roster roster)
        {
            _rosters.Insert(roster);
            await _rosters.Save();
        }

        public async Task DeleteRoster(Roster roster)
        {
            await _rosters.Delete(roster.Id);
            await _rosters.Save();
        }

        public async Task<Roster> GetRosterById(int id)
        {
            return await _rosters.GetById(id);
        }

        public async Task<IEnumerable<Roster>> GetRosters()
        {
            return await _rosters.GetAll();
        }

        public async Task<IEnumerable<Roster>> GetRostersByRoomId(int id)
        {
            return await _rosters.GetAll(m => m.Room_Id == id);
        }

        public async Task UpdateRoster(Roster roster)
        {
            _rosters.Update(roster);
            await _rosters.Save();
        }

        
    }
}
