using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using AnimalDB.Repo.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDB.Repo.Services
{
    public class RackService : IRackService
    {
        private readonly IRepository<Rack> _racks;

        public RackService(IRepository<Rack> racks)
        {
            this._racks = racks;
        }

        public async Task CreateRack(Rack rack)
        {
            _racks.Insert(rack);
            await _racks.Save();
        }

        public async Task DeleteRack(Rack rack)
        {
            await _racks.Delete(rack.Id);
            await _racks.Save();
        }

        public async Task<Rack> GetRackById(int id)
        {
            return await _racks.GetById(id);
        }

        public async Task<IEnumerable<Rack>> GetRacks()
        {
            return await _racks.GetAll();
        }

        public async Task UpdateRack(Rack rack)
        {
            _racks.Update(rack);
            await _racks.Save();
        }

        public async Task<IEnumerable<Rack>> GetRacksByRoomId(int roomId)
        {
            return await _racks.GetAll(m => m.Room_Id == roomId);
        }
    }
}
