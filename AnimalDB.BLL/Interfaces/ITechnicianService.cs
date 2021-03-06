﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AnimalDB.Repo.Entities;

namespace AnimalDB.Repo.Interfaces
{
    public interface ITechnicianService
    {
        Task<IEnumerable<Technician>> GetTechnicians();

        Task<Technician> GetTechnicianByUsername(string username);

        Task CreateTechnician(Technician technician);

        Task<Technician> GetTechnicianById(string id);

        Task UpdateTechnician(Technician technician);

        Task DeleteTechnician(Technician technician);

        Task SetAuthCookie(string userName);
    }
}
