﻿using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Enums;
using AnimalDB.Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize]
    public class AnimalsController : Controller
    {
        private readonly IAnimalService _animals;
        private readonly IArrivalStatusService _arrivalStatus;
        private readonly IColourService _colours;
        private readonly ISourceService _sources;
        private readonly IStrainService _strains;
        private readonly IInvestigatorService _investigators;
        private readonly IStudentService _students;
        private readonly IChargeCodeService _chargeCodes;
        private readonly IRoomService _rooms;
        private readonly IApprovalNumberService _approvalNumbers;
        private readonly IEthicsNumberService _ethicsNumbers;
        private readonly IEthicsNumberHistoryService _ethicsNumberHistories;
        private readonly ITransgeneService _transgenes;
        private readonly IGDTimelineService _gDTimelines;


        public AnimalsController(IAnimalService animals,
                                 IArrivalStatusService arrivalStatus,
                                 IColourService colours,
                                 ISourceService sources,
                                 IStrainService strains,
                                 IInvestigatorService investigators,
                                 IStudentService students,
                                 IChargeCodeService chargeCodes,
                                 IRoomService rooms,
                                 IApprovalNumberService approvalNumbers,
                                 IEthicsNumberService ethicsNumbers,
                                 IEthicsNumberHistoryService ethicsNumberHistories,
                                 ITransgeneService transgenes,
                                 IGDTimelineService gDTimelines
            )
        {
            this._animals = animals;
            this._arrivalStatus = arrivalStatus;
            this._colours = colours;
            this._sources = sources;
            this._strains = strains;
            this._investigators = investigators;
            this._students = students;
            this._chargeCodes = chargeCodes;
            this._rooms = rooms;
            this._approvalNumbers = approvalNumbers;
            this._ethicsNumbers = ethicsNumbers;
            this._ethicsNumberHistories = ethicsNumberHistories;
            this._transgenes = transgenes;
            this._gDTimelines = gDTimelines;
        }

        // GET: /Animals/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }

            DateTime? _surgeryDate = await _animals.GetAnimalsSurgeryDate(id.Value);
            DateTime? _gDDate = await _animals.GetAnimalsGDDate(id.Value);
            DateTime? _injectionDate = await _animals.GetAnimalsInjectionDate(id.Value);
            DateTime? _plugDate = await _animals.GetAnimalsPlugDate(id.Value);
            IEnumerable<Animal> _keptOffspring = await _animals.GetAnimalsLivingOffspring(id.Value);

            ViewBag.AnimalId = id.Value;
            ViewBag.CageLocation = await _animals.GetAnimalsCageLocationDescription(id.Value);
            ViewBag.EthicsNumber = await _animals.GetAnimalsEthicsNumberDescription(id.Value);
            ViewBag.OffspringKept = _keptOffspring.Count();
            ViewBag.PlugDate = _plugDate.ToString();
            ViewBag.GDDescription = await _animals.GetAnimalsGDDescription(id.Value);
            ViewBag.GDDate = _gDDate.ToString();
            ViewBag.InjectionDate = _injectionDate.ToString();
            ViewBag.InjectionBy = await _animals.GetAnimalsInjectionSurgeon(id.Value);
            ViewBag.SurgeryDate = _surgeryDate.ToString();
            ViewBag.Surgeon = await _animals.GetAnimalsSurgeon(id.Value);
            ViewBag.SurgeryType = await _animals.GetAnimalsListOfSurgeries(id.Value);

            return View(animal);
        }

        // GET: /Animals/Barcode
        [Authorize]
        public async Task<ActionResult> Barcode(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }

            var writer = new ZXing.BarcodeWriter()
            {
                Format = ZXing.BarcodeFormat.CODE_39,
                Options = new ZXing.Common.EncodingOptions
                    {
                        Height = 51,
                        Width = 439,
                        PureBarcode = true
                    }
            };

            var image = writer.Write(animal.UniqueAnimalId.ToUpper());
            
            ImageConverter converter = new ImageConverter();
            return File((byte[])converter.ConvertTo(image, typeof(byte[])), "image/bmp");

        }

        [HttpPost]
        public async Task<bool> UploadPhoto(int? id, HttpPostedFileBase file)
        {
            if (id == null)
            {
                return false;
            }
            
            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return false;
            }

            await _animals.AddPhotoToAnimal(animal.Id);

            if (file.ContentLength > 0)
            {
                WebImage img = new WebImage(file.InputStream);
                if (img.Width > 1000)
                {
                    img.Resize(1000, 1000, true, true);
                }

                var path = Path.Combine(Server.MapPath("~/Content/AnimalImages"), id.ToString() + ".jpg");
                img.Save(path);
            }

            return true;
        }

        [HttpPost]
        public async Task<bool> RemovePhoto(int? id)
        {
            if (id == null)
            {
                return false;
            }
            
            var animal = await _animals.GetAnimalById(id.Value);

            if (animal == null)
            {
                return false;
            }

            await _animals.RemovePhotoFromAnimal(id.Value);

            var path = Path.Combine(Server.MapPath("~/Content/AnimalImages"), id.ToString() + ".jpg");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return true;
        }

        // GET: /Animals/Create
        [Authorize(Roles = "Administrator, Technician")]
        public async Task<ActionResult> Create()
        {
            ViewBag.ArrivalStatus_Id = new SelectList(await _arrivalStatus.GetArrivalStatus(), "Id", "Description");
            ViewBag.Colour_Id = new SelectList(await _colours.GetColours(), "Id", "Description");
            ViewBag.Source_Id = new SelectList(await _sources.GetSources(), "Id", "Description");
            ViewBag.Strain_Id = new SelectList(await _strains.GetStrains(), "Id", "Description");
            ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName");
            ViewBag.Researcher_Id = new SelectList(await _students.GetStudentsAndInvestigators(), "Id", "FullName");
            //ViewBag.ChargeCode_Id = new SelectList(_chargeCodes.GetChargeCodes(), "Id", "Text");
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description");
            ViewBag.ApprovalNumber_Id = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description");
            ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text");
            ViewBag.Transgene_Id = new SelectList(await _transgenes.GetTransgenes(), "Id", "Description");
            ViewBag.GDTimeline_Id = new SelectList(await _gDTimelines.GetGDTimelines(), "Id", "Description");

            var model = new Animal()
            {
                Manipulation = Manipulation.BasicBiologicalResearch,
                Grading = Grading.ModerateSuffering,
                StockAnimal = false
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Technician")]
        public async Task<ActionResult> Create(Animal animal, int? Ethics_Id, string next)
        {
            if (await _animals.GetAnimalByUniqueId(animal.UniqueAnimalId) != null)
            {
                ModelState.AddModelError("UniqueAnimalId", "Unique Animal Id is already in use.");
            }

            if (animal.CauseOfDeath != null && animal.DeathDate == null)
            {
                ModelState.AddModelError("DeathDate", "For an animal to be marked as deceased you must specify the date of death");
            }

            if (animal.CauseOfDeath == null && animal.DeathDate != null)
            {
                ModelState.AddModelError("CauseOfDeath", "For an animal to be marked as deceased you must specify the cause of death");
            }

            if (!animal.StockAnimal && Ethics_Id == null)
            {
                ModelState.AddModelError("Ethics_Id", "You must specify an AUP number");
            }

            if (ModelState.IsValid)
            {
                animal.LastChecked = DateTime.Now;
                await _animals.CreateAnimal(animal);

                if (Ethics_Id.HasValue)
                {
                    await _ethicsNumberHistories
                        .CreateEthicsNumberHistory(new EthicsNumberHistory()
                    {
                        Animal_Id = animal.Id,
                        Ethics_Id = Ethics_Id.Value,
                        Timestamp = DateTime.Now
                    });
                }
                if (next == "finish")
                {
                    if (animal.DeathDate.HasValue)
                    {
                        return RedirectToAction("Details", "Culled", new { id = animal.Id, returnUrl = Url.Action("Index", "Culled") });
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = animal.Id });
                    }
                }
                else
                {
                    ViewBag.SuccessMsg = "Animal " + animal.UniqueAnimalId + " created successfully.";
                    animal.UniqueAnimalId = "";
                }
            }

            ViewBag.ArrivalStatus_Id = new SelectList(await _arrivalStatus.GetArrivalStatus(), "Id", "Description", animal.ArrivalStatus_Id);
            ViewBag.Colour_Id = new SelectList(await _colours.GetColours(), "Id", "Description", animal.Colour_Id);
            ViewBag.Source_Id = new SelectList(await _sources.GetSources(), "Id", "Description", animal.Source_Id);
            ViewBag.Strain_Id = new SelectList(await _strains.GetStrains(), "Id", "Description", animal.Strain_Id);
            ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName", animal.Investigator_Id);
            ViewBag.Researcher_Id = new SelectList(await _students.GetStudentsAndInvestigators(), "Id", "FullName", animal.Researcher_Id);
            //ViewBag.ChargeCode_Id = new SelectList(_chargeCodes.GetChargeCodes(), "Id", "Text", animal.ChargeCode_Id);
            ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", animal.Room_Id);
            ViewBag.ApprovalNumber_Id = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description", animal.ApprovalNumber_Id);
            ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text", Ethics_Id);
            ViewBag.Transgene_Id = new SelectList(await _transgenes.GetTransgenes(), "Id", "Description", animal.Transgene_Id);
            return View(animal);
        }

        // GET: /Animals/Edit/5
        [Authorize(Roles = "Student, Administrator, Technician, Investigator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Student"))
            {
                ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", animal.Room_Id);
                ViewBag.ApprovalNumber_Id = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description", animal.ApprovalNumber_Id);
                ViewBag.Colour_Id = new SelectList(await _colours.GetColours(), "Id", "Description", animal.Colour_Id);
                return View("StudentEdit", animal);
            }
            else 
            {
                ViewBag.ArrivalStatus_Id = new SelectList(await _arrivalStatus.GetArrivalStatus(), "Id", "Description", animal.ArrivalStatus_Id);
                ViewBag.Colour_Id = new SelectList(await _colours.GetColours(), "Id", "Description", animal.Colour_Id);
                ViewBag.Source_Id = new SelectList(await _sources.GetSources(), "Id", "Description", animal.Source_Id);
                ViewBag.Strain_Id = new SelectList(await _strains.GetStrains(), "Id", "Description", animal.Strain_Id);
                ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName", animal.Investigator_Id);
                ViewBag.Researcher_Id = new SelectList(await _students.GetStudentsAndInvestigators(), "Id", "FullName", animal.Researcher_Id);
                //ViewBag.ChargeCode_Id = new SelectList(_chargeCodes.GetChargeCodes(), "Id", "Text", animal.ChargeCode_Id);
                ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", animal.Room_Id);
                ViewBag.ApprovalNumber_Id = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description", animal.ApprovalNumber_Id);
                ViewBag.Transgene_Id = new SelectList(await _transgenes.GetTransgenes(), "Id", "Description", animal.Transgene_Id);
                var ethics = await _animals.GetAnimalsEthicsNumber(animal.Id);
                ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text", ethics?.Ethics_Id);
                return View(animal);
            }
        }

        // POST: /Animals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student, Administrator, Technician, Investigator")]
        public async Task<ActionResult> Edit(Animal animal, int? Ethics_Id)
        {
            if (await _animals.GetAnimalByUniqueId(animal.UniqueAnimalId) != null && _animals.GetAnimalByUniqueId(animal.UniqueAnimalId).Id != animal.Id)
            {
                ModelState.AddModelError("UniqueAnimalId", "Unique Animal Id is already in use.");
            }
            
            if (animal.CauseOfDeath != null && animal.DeathDate == null)
            {
                ModelState.AddModelError("DeathDate", "For an animal to be marked as deceased you must specify the date of death");
            }

            if (animal.CauseOfDeath == null && animal.DeathDate != null)
            {
                ModelState.AddModelError("CauseOfDeath", "For an animal to be marked as deceased you must specify the cause of death");
            }

            if (!User.IsInRole("Student") && !animal.StockAnimal && !Ethics_Id.HasValue)
            {
                ModelState.AddModelError("Ethics_Id", "You must specify an AUP number");
            }

            if (ModelState.IsValid)
            {
                if (animal.DeathDate != null)
                {
                    animal.FeedingGroup_Id = null;
                    animal.Group_Id = null;
                }
                await _animals.UpdateAnimal(animal);

                if (Ethics_Id.HasValue)
                {
                    await _animals.AddAnimalToEthicsNumber(animal.Id, Ethics_Id.Value);
                }

                if (animal.DeathDate.HasValue)
                {
                    return RedirectToAction("Details", "Culled", new { id = animal.Id, returnUrl = Url.Action("Index", "Culled") });
                }
                else
                {
                    return RedirectToAction("Details", new { id = animal.Id });
                }
            }

            if (User.IsInRole("Student"))
            {
                ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", animal.Room_Id);
                ViewBag.ApprovalNumber_Id = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description", animal.ApprovalNumber_Id);
                ViewBag.Colour_Id = new SelectList(await _colours.GetColours(), "Id", "Description", animal.Colour_Id);
                return View("StudentEdit", animal);
            }
            else
            {
                ViewBag.ArrivalStatus_Id = new SelectList(await _arrivalStatus.GetArrivalStatus(), "Id", "Description", animal.ArrivalStatus_Id);
                ViewBag.Colour_Id = new SelectList(await _colours.GetColours(), "Id", "Description", animal.Colour_Id);
                ViewBag.Source_Id = new SelectList(await _sources.GetSources(), "Id", "Description", animal.Source_Id);
                ViewBag.Strain_Id = new SelectList(await _strains.GetStrains(), "Id", "Description", animal.Strain_Id);
                ViewBag.Investigator_Id = new SelectList(await _investigators.GetInvestigators(), "Id", "FullName", animal.Investigator_Id);
                ViewBag.Researcher_Id = new SelectList(await _students.GetStudentsAndInvestigators(), "Id", "FullName", animal.Researcher_Id);
                //ViewBag.ChargeCode_Id = new SelectList(_chargeCodes.GetChargeCodes(), "Id", "Text", animal.ChargeCode_Id);
                ViewBag.Room_Id = new SelectList(await _rooms.GetRooms(), "Id", "Description", animal.Room_Id);
                ViewBag.ApprovalNumber_Id = new SelectList(await _approvalNumbers.GetApprovalNumbers(), "Id", "Description", animal.ApprovalNumber_Id);
                ViewBag.Transgene_Id = new SelectList(await _transgenes.GetTransgenes(), "Id", "Description", animal.Transgene_Id);
                var ethics = await _animals.GetAnimalsEthicsNumber(animal.Id);
                ViewBag.Ethics_Id = new SelectList(await _ethicsNumbers.GetEthicsNumbers(), "Id", "Text", ethics?.Ethics_Id);
                return View(animal);
            }
        }

        // GET: /Animals/Delete/5
        [Authorize(Roles = "Administrator, Technician")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }


        // POST: /Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Technician")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Animal animal = await _animals.GetAnimalById(id);
            await _animals.DeleteAnimal(animal);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Animals/Resurrect/5
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Resurrect(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = await _animals.GetAnimalById(id.Value);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: /Animals/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Resurrect(int id)
        {
            await _animals.Resurrect(id);
            return RedirectToAction("Details", "Animals", new { id });
        }
    }
}
