﻿using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using AnimalDB.Repo.Implementations;
using AnimalDB.Models;
using AnimalDB.Web.Models;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Investigator, Technician, Administrator")]
    public class AnimalManipulationReportsController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IAnimalManipulationReport _animalManipulationReports;
        private IEthicsNumber _ethicsNumbers;
        private IEthicsNumberHistory _ethicsNumberHistories;
        private IAnimal _animals;
        private IArrivalStatus _arrivalStatus;

        public AnimalManipulationReportsController()
        {
            this._animalManipulationReports = new AnimalManipulationReportRepo();
            this._ethicsNumbers = new EthicsNumberRepo();
            this._ethicsNumberHistories = new EthicsNumberHistoryRepo();
            this._animals = new AnimalRepo();
            this._arrivalStatus = new ArrivalStatusRepo();
        }

        // GET: AnimalManipulationReports
        public ActionResult Index()
        {
            //var animalManipulationReports = db.AnimalManipulationReports.Include(a => a.Investigator).Include(a => a.Species);
            return View(_animalManipulationReports.GetAnimalManipulationReports());
        }

        // GET: AnimalManipulationReports/SelectEthics
        public ActionResult SelectEthics()
        {
            ViewBag.Ethics_Id = new SelectList(_ethicsNumbers.GetEthicsNumbers(), "Id", "Text");
            return View();
        }

        // GET: AnimalManipulationReports/Create
        public async Task<ActionResult> Create(int? Ethics_Id)
        {
            if (Ethics_Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ethics = await _ethicsNumbers.GetEthicsNumberById(Ethics_Id.Value);
            var ethicsNumberHistories = ethics.EthicsNumberHistory;
            if (ethics == null)
            {
                return HttpNotFound();
            }

            var model = new AnimalManipulationReport()
            {
                PeriodFrom = new DateTime(ethics.StartYear.Value, 1, 1),
                PeriodTo = new DateTime(ethics.StartYear.Value, 12, 31).AddYears(2),
                Investigator = ethics.Investigator,
                Investigator_Id = ethics.Investigator.Id,
                ProtocolNumber = ethics.Text,
                Species = ethics.Species,
                Species_Id = ethics.Species_Id.Value,

                BornDuringProject = ethicsNumberHistories.Count(m => m.Animal.Source.Type == SourceType.BornDuringProject),
                BreedingUnit = ethicsNumberHistories.Count(m => m.Animal.Source.Type == SourceType.BreedingUnit && !m.Animal.BornHere),
                Captured = ethicsNumberHistories.Count(m => m.Animal.Source.Type == SourceType.Captured && !m.Animal.BornHere),
                Commercial = ethicsNumberHistories.Count(m => m.Animal.Source.Type == SourceType.Commercial && !m.Animal.BornHere),
                Farm = ethicsNumberHistories.Count(m => m.Animal.Source.Type == SourceType.Farm && !m.Animal.BornHere),
                Imported = ethicsNumberHistories.Count(m => m.Animal.Source.Type == SourceType.ImportedIntoNZ && !m.Animal.BornHere),
                PublicSources = ethicsNumberHistories.Count(m => m.Animal.Source.Type == SourceType.PublicSources && !m.Animal.BornHere),

                Diseased = ethicsNumberHistories.Count(m => m.Animal.ArrivalStatus.Type == ArrivalStatusType.Diseased),
                NormalConventional = ethicsNumberHistories.Count(m => m.Animal.ArrivalStatus.Type == ArrivalStatusType.Normal_Conventional),
                OtherStatus = ethicsNumberHistories.Count(m => m.Animal.ArrivalStatus.Type == ArrivalStatusType.Other),
                ProtectedSpecies = ethicsNumberHistories.Count(m => m.Animal.ArrivalStatus.Type == ArrivalStatusType.ProtectedSpecies),
                SPFGermFree = ethicsNumberHistories.Count(m => m.Animal.ArrivalStatus.Type == ArrivalStatusType.SPF_GermFree),
                TransgenicChimera = ethicsNumberHistories.Count(m => m.Animal.ArrivalStatus.Type == ArrivalStatusType.Transgenic_Chimera),
                UnbornPrehatched = ethicsNumberHistories.Count(m => m.Animal.ArrivalStatus.Type == ArrivalStatusType.Unborn_Prehatched),

                AnimalHusbandry = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.AnimalHusbandry),
                BasicBiologicalResearch = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.BasicBiologicalResearch),
                DevelopmentOfAlternatives = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.DevelopmentOfAlternatives),
                EnvironmentalManagement = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.EnvironmentalManagement),
                MedicalResearch = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.MedicalResearch),
                OtherManipulation = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.Other),
                ProducingOffspringWithPotentialForCompromisedWelfare = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.ProducingOffspringWithPotentialForCompromisedWelfare),
                ProductionOfBiologicalAgents = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.ProductionOfBiologicalAgents),
                SpeciesConservation = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.SpeciesConservation),
                Teaching = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.Teaching),
                Testing = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.Testing),
                VeterinaryResearch = ethicsNumberHistories.Count(m => m.Animal.Manipulation == Manipulation.VeterinaryResearch),

                NoPriorUse = ethicsNumberHistories.Count(m => m.Animal.EthicsNumbers.Count() == 1),
                PreviouslyUsed = ethicsNumberHistories.Count(m => m.Animal.EthicsNumbers.Count() != 1),

                NoSuffering = ethicsNumberHistories.Count(m => m.Animal.Grading == Grading.NoSuffering),
                LittleSuffering = ethicsNumberHistories.Count(m => m.Animal.Grading == Grading.LittleSuffering),
                ModerateSuffering = ethicsNumberHistories.Count(m => m.Animal.Grading == Grading.ModerateSuffering),
                SevereSuffering = ethicsNumberHistories.Count(m => m.Animal.Grading == Grading.SevereSuffering),
                VerySevereSuffering = ethicsNumberHistories.Count(m => m.Animal.Grading == Grading.VerySevereSuffering),

                DisposedOf = ethicsNumberHistories.Count(m => m.AliveStatus == AliveStatus.DisposedOf_ToOthers),
                ReleasedToTheWild = ethicsNumberHistories.Count(m => m.AliveStatus == AliveStatus.Released_ToTheWild),
                RetainedByInstitution = ethicsNumberHistories.Count(m => m.AliveStatus == AliveStatus.Retained_ByYourInstitution),
                ReturnedToOwner = ethicsNumberHistories.Count(m => m.AliveStatus == AliveStatus.Returned_ToOwner),

                TotalDead = ethicsNumberHistories.Count(m => m.Animal.DeathDate.HasValue),

                TotalSource = ethicsNumberHistories.Count(),

                TotalUsed = ethicsNumberHistories.Count(),

                TotalAlive = ethicsNumberHistories.Count(m => !m.Animal.DeathDate.HasValue)
            };


            model.BornDuringProject += ethicsNumberHistories.Count(m => m.Animal.BornHere);

            model.NilReturn = model.TotalSource - model.TotalAlive - model.TotalDead;

            return View(model);
        }

        // POST: AnimalManipulationReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AnimalManipulationReport animalManipulationReport)
        {
            animalManipulationReport.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _animalManipulationReports.CreateAnimalManipulationReport(animalManipulationReport);
                return RedirectToAction("Index");
            }

            return View(animalManipulationReport);
        }

        // GET: AnimalManipulationReports/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalManipulationReport animalManipulationReport = await _animalManipulationReports.GetAnimalManipulationReportById(id.Value);
            if (animalManipulationReport == null)
            {
                return HttpNotFound();
            }

            var ethicsNumber = await _ethicsNumbers.GetEthicsNumberByName(animalManipulationReport.ProtocolNumber);
            ViewBag.Ethics_Id = ethicsNumber.Id;

            return View(animalManipulationReport);
        }

        // POST: AnimalManipulationReports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AnimalManipulationReport animalManipulationReport)
        {
            if (ModelState.IsValid)
            {
                await _animalManipulationReports.UpdateAnimalManipulationReport(animalManipulationReport);
                return RedirectToAction("Index");
            }
            var ethicsNumber = await _ethicsNumbers.GetEthicsNumberByName(animalManipulationReport.ProtocolNumber);
            ViewBag.Ethics_Id = ethicsNumber.Id;
            return View(animalManipulationReport);
        }

        // GET: AnimalManipulationReports/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalManipulationReport animalManipulationReport = await _animalManipulationReports.GetAnimalManipulationReportById(id.Value);
            if (animalManipulationReport == null)
            {
                return HttpNotFound();
            }
            return View(animalManipulationReport);
        }

        // POST: AnimalManipulationReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AnimalManipulationReport animalManipulationReport = await _animalManipulationReports.GetAnimalManipulationReportById(id);
            await _animalManipulationReports.DeleteAnimalManipulationReport(animalManipulationReport);
            return RedirectToAction("Index");
        }

        // GET: AnimalManipulationReports/View/?category=SourceType&identifier=BreedingUnit
        public ActionResult ViewAnimals(int ethics_Id, string category, string identifier)
        {
            var model = _animals
                .GetAllAnimals()
                .Where(m => m.EthicsNumbers.Count(n => n.Ethics_Id == ethics_Id) != 0)
                .Select(m => new ViewAnimalManipulationReportViewModel() { Animal = m, Change = false });

            switch (category)
            {
                case "sourceType":
                    if (identifier == SourceType.BornDuringProject.ToString())
                    {
                        model = model.Where(m => m.Animal.BornHere || m.Animal.Source.Type.ToString() == identifier);
                    }
                    else
                    {
                        model = model.Where(m => !m.Animal.BornHere && m.Animal.Source.Type.ToString() == identifier);
                    }

                    break;
                case "arrivalStatusType":
                    ArrivalStatusType arrivalStatusType;
                    Enum.TryParse(identifier, out arrivalStatusType);
                    model = model.Where(m => m.Animal.ArrivalStatus.Type == arrivalStatusType);
                    break;
                case "manipulation":
                    Manipulation manipulation;
                    Enum.TryParse(identifier, out manipulation);
                    model = model.Where(m => m.Animal.Manipulation == manipulation);
                    break;
                case "priorUse":
                    int _identifier = identifier == "yes" ? 2 : 1;
                    model = model.Where(m => m.Animal.EthicsNumbers.Count() == _identifier);
                    break;
                case "grading":
                    Grading grading;
                    Enum.TryParse(identifier, out grading);
                    model = model.Where(m => m.Animal.Grading == grading);
                    break;
                case "aliveStatus":
                    AliveStatus aliveStatus;
                    Enum.TryParse(identifier, out aliveStatus);
                    model = model.Where(m => _animals.GetAnimalsEthicsNumber(m.Animal.Id).Result.AliveStatus == aliveStatus);
                    break;
            }

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ViewResult> BulkChange(string[] animalIds, string returnUrl, string ethics_Id)
        {
            List<Animal> animals = new List<Animal>();

            foreach (var id in animalIds)
            {
                int _id = Convert.ToInt32(id);
                animals.Add(
                    await _animals.GetAnimalById(_id)
                    );
            }

            var model = new BulkChangeAnimalViewModel()
            {
                Animals = animals
            };

            ViewBag.returnUrl = returnUrl;
            ViewBag.ArrivalStatus_Id = new SelectList(_arrivalStatus.GetArrivalStatus(), "Id", "Description");

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BulkChange(BulkChangeAnimalViewModel model, string[] animalIds, string returnUrl, string ethics_Id)
        {
            List<Animal> animals = new List<Animal>();

            foreach (var id in animalIds)
            {
                int _id = Convert.ToInt32(id);
                animals.Add(
                    await _animals.GetAnimalById(_id)
                    );
            }

            await _animals.BulkUpdateAnimals(animals, model.Grading, model.Purpose, model.ArrivalStatus_Id);

            return RedirectToAction("Create", new { ethics_Id });
        }
    }
}
