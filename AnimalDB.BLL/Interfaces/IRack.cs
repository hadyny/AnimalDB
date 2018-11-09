﻿using AnimalDB.Repo.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Interfaces
{
    public interface IRack
    {
        IEnumerable<Rack> GetRacks();

        Task CreateRack(Rack rack);

        Task<Rack> GetRackById(int id);

        IEnumerable<Rack> GetRacksByRoomId(int roomId);

        Task UpdateRack(Rack rack);

        Task DeleteRack(Rack rack);
    }
}
