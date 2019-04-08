using   AnimalDBCore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace   AnimalDBCore.Core.Interfaces
{
    public interface IRoster
    {
        IEnumerable<Roster> GetRosters();

        Task CreateRoster(Roster roster);

        Task<Roster> GetRosterById(int id);

        Task UpdateRoster(Roster roster);

        Task DeleteRoster(Roster roster);

        IEnumerable<Roster> GetRostersByRoomId(int id);
    }
}
