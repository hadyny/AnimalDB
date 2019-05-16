﻿using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Services;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TechnicianController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private readonly ITechnicianService _technicians;
        private readonly IUserManagementService _users;

        public TechnicianController(ITechnicianService technicians, IUserManagementService users)
        {
            this._technicians = technicians;
            this._users = users;
        }

        // GET: /Technician/
        public async Task<ActionResult> Index()
        {
            return View(await _technicians.GetTechnicians());
        }

        // GET: /Technician/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="UserName,Email,FirstName,LastName")] Technician technician)
        {
            string error = await _users.CreateAnimalUser(technician, Repo.Enums.UserType.Technician);

            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", error);
            }

            return View(technician);
        }

        // GET: /Technician/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Technician Technician = await _technicians.GetTechnicianById(id);
            if (Technician == null)
            {
                return HttpNotFound();
            }
            return View(Technician);
        }

        // POST: /Technician/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Technician technician = await _technicians.GetTechnicianById(id);
            await _technicians.DeleteTechnician(technician);
            return RedirectToAction("Index");
        }
    }
}
