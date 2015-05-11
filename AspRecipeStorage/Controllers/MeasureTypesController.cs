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
    public class MeasureTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MeasureTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.MeasureTypes.ToListAsync());
        }

        // GET: MeasureTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeasureType measureType = await db.MeasureTypes.FindAsync(id);
            if (measureType == null)
            {
                return HttpNotFound();
            }
            return View(measureType);
        }

        // GET: MeasureTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeasureTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] MeasureType measureType)
        {
            if (ModelState.IsValid)
            {
                db.MeasureTypes.Add(measureType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(measureType);
        }

        // GET: MeasureTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeasureType measureType = await db.MeasureTypes.FindAsync(id);
            if (measureType == null)
            {
                return HttpNotFound();
            }
            return View(measureType);
        }

        // POST: MeasureTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] MeasureType measureType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(measureType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(measureType);
        }

        // GET: MeasureTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeasureType measureType = await db.MeasureTypes.FindAsync(id);
            if (measureType == null)
            {
                return HttpNotFound();
            }
            return View(measureType);
        }

        // POST: MeasureTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MeasureType measureType = await db.MeasureTypes.FindAsync(id);
            db.MeasureTypes.Remove(measureType);
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
