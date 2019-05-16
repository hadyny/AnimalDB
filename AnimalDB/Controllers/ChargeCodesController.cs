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
        IChargeCodeService _chargeCodes;

        public ChargeCodesController(IChargeCodeService chargeCodes)
        {
            this._chargeCodes = chargeCodes;
        }

        // GET: ChargeCodes
        public async Task<ActionResult> Index()
        {
            return View(await _chargeCodes.GetChargeCodes());
        }

        // GET: ChargeCodes/Animals
        public async Task<ActionResult> Animals(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChargeCode chargeCode = await _chargeCodes.GetChargeCodeById(id.Value);
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
            var codes = await _chargeCodes.GetChargeCodes();
            if (codes.Count(m => m.Text == chargeCode.Text) != 0)
            {
                ModelState.AddModelError("", "Charge code already in use");
            }

            if (ModelState.IsValid)
            {
                await _chargeCodes.CreateChargeCode(chargeCode);
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
            ChargeCode chargeCode = await _chargeCodes.GetChargeCodeById(id.Value);
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
            var codes = await _chargeCodes.GetChargeCodes();
            if (codes.Count(m => m.Text == chargeCode.Text) != 0)
            {
                ModelState.AddModelError("", "Charge code already in use");
            }

            if (ModelState.IsValid)
            {
                await _chargeCodes.UpdateChargeCode(chargeCode);
                return RedirectToAction("Index");
            }
            return View(chargeCode);
        }
    }
}
