using AnimalDB.Repo.Contexts;
using AnimalDB.Repo.Entities;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class ColourController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ColourController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Colour/
        public async Task<ActionResult> Index()
        {
            return View(await _unitOfWork.Colours.Get());
        }


        // GET: /Colour/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Colour/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Description")] Colour colour)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Colours.Insert(colour);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(colour);
        }

        // GET: /Colour/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colour colour = await _unitOfWork.Colours.GetById(id.Value);
            if (colour == null)
            {
                return HttpNotFound();
            }
            return View(colour);
        }

        // POST: /Colour/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Description")] Colour colour)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Colours.Update(colour);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(colour);
        }

        // GET: /Colour/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colour colour = await _unitOfWork.Colours.GetById(id.Value);
            if (colour == null)
            {
                return HttpNotFound();
            }
            return View(colour);
        }

        // POST: /Colour/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Colour colour = await _unitOfWork.Colours.GetById(id);
            _unitOfWork.Colours.Delete(colour);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
            

            
        }
    }
}
