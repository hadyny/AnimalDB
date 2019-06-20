using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Interfaces;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ChargeCodesController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;

        public ChargeCodesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: ChargeCodes
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.ChargeCodes.Get());
        }

        // GET: ChargeCodes/Animals
        public async Task<ActionResult> Animals(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChargeCode chargeCode = await _unitOfWork.ChargeCodes.GetById(id.Value);
            if (chargeCode == null)
            {
                return HttpNotFound();
            }

            ViewBag.ChargeCode = chargeCode.Text;

            return View(chargeCode.Animals);
        }
       
        // GET: ChargeCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChargeCodes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Text")] ChargeCode chargeCode)
        {
            var codes = await _unitOfWork.ChargeCodes.Get();
            if (codes.Count(m => m.Text == chargeCode.Text) != 0)
            {
                ModelState.AddModelError("", "Charge code already in use");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ChargeCodes.Insert(chargeCode);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(chargeCode);
        }

        // GET: ChargeCodes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChargeCode chargeCode = await _unitOfWork.ChargeCodes.GetById(id.Value);
            if (chargeCode == null)
            {
                return HttpNotFound();
            }
            return View(chargeCode);
        }

        // POST: ChargeCodes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Text")] ChargeCode chargeCode)
        {
            var codes = await _unitOfWork.ChargeCodes.Get();
            if (codes.Count(m => m.Text == chargeCode.Text) != 0)
            {
                ModelState.AddModelError("", "Charge code already in use");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.ChargeCodes.Update(chargeCode);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(chargeCode);
        }
    }
}
