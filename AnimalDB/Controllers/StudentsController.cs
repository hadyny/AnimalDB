﻿using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator,Technician")]
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagementService _userManagementService;

        public StudentsController(IUnitOfWork unitOfWork, IUserManagementService userManagementService)
        {
            _unitOfWork = unitOfWork;
            _userManagementService = userManagementService;
        }

        // GET: Students
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Students.Get());
        }

        // GET: Students/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Investigator_Id = new SelectList(await _unitOfWork.Investigators.Get(), "Id", "FullName");
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Investigator_Id,UserName,Email,FirstName,LastName")] Student student)
        {
            string error = await _userManagementService.Register(student, Repo.Enums.UserType.Student);

            if (string.IsNullOrEmpty(error))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", error);
            }

            return View(student);
        }

        // GET: Students/AddInvestigator/5
        public async Task<ActionResult> AddInvestigator(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var model = new Models.AddInvestigatorVM
            {
                StudentId = student.Id,
                Investigators = student.Investigators
            };
            ViewBag.StudentName = student.FullName;
            var inv = await _unitOfWork.Investigators.Get();
            List <Investigator> investigators = inv.Where(m => !m.Students.Contains(student)).ToList();
            ViewBag.Selected_Investigator_Id = new SelectList(investigators, "Id", "FullName");

            return View(model);
        }

        // POST: Students/AddInvestigator/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddInvestigator(Models.AddInvestigatorVM model)
        {
            // Just this needed to be implemented, and removing functions then update database and test creating students, adding
            // investigators, removing investigators, and student access
            Student student = await _unitOfWork.Students.GetById(model.StudentId);

            if (ModelState.IsValid)
            {
                await _unitOfWork.Students.AddInvestigatorToStudent(studentId: model.StudentId, investigatorId: model.Selected_Investigator_Id);
                await _unitOfWork.Complete();
            }

            ViewBag.StudentName = student.FullName;
            var inv = await _unitOfWork.Investigators.Get();
            ViewBag.Selected_Investigator_Id = new SelectList(inv.Where(m => !m.Students.Contains(student)), "Id", "FullName");
            model.Investigators = student.Investigators;
            return View(model);
        }

        // GET: Students/RemoveInvestigator/5?inv=6
        public async Task<ActionResult> RemoveInvestigator(string id, string inv)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(inv))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var student = await _unitOfWork.Students.GetById(id);

            var investigtor = await _unitOfWork.Investigators.GetById(inv);

            if (student == null || investigtor == null)
            {
                return HttpNotFound();
            }
            
            await _unitOfWork.Students.RemoveInvestigatorFromStudent(studentId: student.Id, investigatorId: investigtor.Id);
            await _unitOfWork.Complete();

            return RedirectToAction("AddInvestigator", new { id });
        }

        // GET: Students/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Student student = await _unitOfWork.Students.GetById(id);
            await _unitOfWork.Students.Delete(student);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
