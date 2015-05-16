using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspRecipeStorage.Models;

namespace AspRecipeStorage.Controllers
{
    public class InstrumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ChangeVisibilityStatus(int id)
        {
            Instrument instr = db.Instruments.FirstOrDefault(i => i.Id == id);
            instr.ForAllUsers = !instr.ForAllUsers;
            db.Entry(instr).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Instruments
        public async Task<ActionResult> Index()
        {
            return View(await db.Instruments.OrderByDescending(i => i.ForAllUsers).ToListAsync());
        }

        // GET: Instruments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = await db.Instruments.FindAsync(id);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        public bool ValidateInstrument(Instrument instrument)
        {
            instrument.Name = instrument.Name.ToLower();
            if (db.Instruments.Where(i => i.Name == instrument.Name).Count() > 0)
            {
                ViewBag.WarningMessage = "Операция отклонена, т.к. введенное название инструмента уже зарегистрированно.";
                return false;
            }
            return true;
        }

        // GET: Instruments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Instrument instrument)
        {
            if (ModelState.IsValid && this.ValidateInstrument(instrument))
            {
                db.Instruments.Add(instrument);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = await db.Instruments.FindAsync(id);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Instrument instrument)
        {
            if (ModelState.IsValid && this.ValidateInstrument(instrument))
            {
                db.Entry(instrument).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(instrument);
        }

        // GET: Instruments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instrument instrument = await db.Instruments.FindAsync(id);
            if (instrument == null)
            {
                return HttpNotFound();
            }
            return View(instrument);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Instrument instrument = await db.Instruments.FindAsync(id);
            db.Instruments.Remove(instrument);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
