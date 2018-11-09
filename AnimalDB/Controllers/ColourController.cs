using AnimalDB.Repo.Entities;
using AnimalDB.Repo.Implementations;
using AnimalDB.Repo.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AnimalDB.Controllers
{
    [Authorize(Roles = "Technician, Administrator")]
    public class ColourController : Controller
    {
        //private AnimalDBContext db = new AnimalDBContext();
        private IColour _colours;

        public ColourController()
        {
            this._colours = new ColourRepo();
        }

        // GET: /Colour/
        public ActionResult Index()
        {
            return View(_colours.GetColours());
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
                await _colours.CreateColour(colour);
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
            Colour colour = await _colours.GetColourById(id.Value);
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
                await _colours.UpdateColour(colour);
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
            Colour colour = await _colours.GetColourById(id.Value);
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
            Colour colour = await _colours.GetColourById(id);
            await _colours.DeleteColour(colour);
            return RedirectToAction("Index");
            

            
        }
    }
}
