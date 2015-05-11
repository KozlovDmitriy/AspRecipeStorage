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
    public class MeasureConversionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MeasureConversions
        public async Task<ActionResult> Index()
        {
            var measureConversions = db.MeasureConversions.Include(m => m.MeasureSecondType).Include(m => m.MeasureFirstType).Include(m => m.IngredientType);
            return View(await measureConversions.ToListAsync());
        }

        // GET: MeasureConversions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeasureConversion measureConversion = await db.MeasureConversions.FindAsync(id);
            if (measureConversion == null)
            {
                return HttpNotFound();
            }
            return View(measureConversion);
        }

        private bool ValidateConversion(MeasureConversion conversion)
        {
            if (
                conversion.MeasureTypeFirstId == conversion.MeasureTypeSecondId ||
                db.MeasureConversions.Where(i => 
                    (
                        i.MeasureTypeFirstId == conversion.MeasureTypeFirstId &&
                        i.MeasureTypeSecondId == conversion.MeasureTypeSecondId &&
                        i.IngredientTypeId == conversion.IngredientTypeId &&
                        i.Id != conversion.Id
                    ) 
                    ||
                    (
                        i.MeasureTypeSecondId == conversion.MeasureTypeFirstId &&
                        i.MeasureTypeFirstId == conversion.MeasureTypeSecondId &&
                        i.IngredientTypeId == conversion.IngredientTypeId &&
                        i.Id != conversion.Id
                    )
                ).Count() > 0
            )
            {
                ViewBag.WarningMessage = "Операция отклонена, т.к. соотношение между двумя выбранными мерами для выбранного ингредиента уже существует.";
                return false;
            }
            return true;
        }

        // GET: MeasureConversions/Create
        public ActionResult Create()
        {
            ViewBag.MeasureTypeSecondId = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.MeasureTypeFirstId = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.IngredientTypeId = new SelectList(db.IngredientTypes, "Id", "Name");
            return View();
        }

        // POST: MeasureConversions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MeasureTypeFirstId,MeasureTypeSecondId,Ratio,IngredientTypeId")] MeasureConversion measureConversion)
        {
            if ( ModelState.IsValid && ValidateConversion(measureConversion) )
            {
                db.MeasureConversions.Add(measureConversion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MeasureTypeSecondId = new SelectList(db.MeasureTypes, "Id", "Name", measureConversion.MeasureTypeSecondId);
            ViewBag.MeasureTypeFirstId = new SelectList(db.MeasureTypes, "Id", "Name", measureConversion.MeasureTypeFirstId);
            ViewBag.IngredientTypeId = new SelectList(db.IngredientTypes, "Id", "Name", measureConversion.IngredientTypeId);
            return View(measureConversion);
        }

        // GET: MeasureConversions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeasureConversion measureConversion = await db.MeasureConversions.FindAsync(id);
            if (measureConversion == null)
            {
                return HttpNotFound();
            }
            ViewBag.MeasureTypeSecondId = new SelectList(db.MeasureTypes, "Id", "Name", measureConversion.MeasureTypeSecondId);
            ViewBag.MeasureTypeFirstId = new SelectList(db.MeasureTypes, "Id", "Name", measureConversion.MeasureTypeFirstId);
            ViewBag.IngredientTypeId = new SelectList(db.IngredientTypes, "Id", "Name", measureConversion.IngredientTypeId);
            return View(measureConversion);
        }

        // POST: MeasureConversions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MeasureTypeFirstId,MeasureTypeSecondId,Ratio,IngredientTypeId")] MeasureConversion measureConversion)
        {
            if ( ModelState.IsValid && ValidateConversion(measureConversion) )
            {
                db.Entry(measureConversion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MeasureTypeSecondId = new SelectList(db.MeasureTypes, "Id", "Name", measureConversion.MeasureTypeSecondId);
            ViewBag.MeasureTypeFirstId = new SelectList(db.MeasureTypes, "Id", "Name", measureConversion.MeasureTypeFirstId);
            ViewBag.IngredientTypeId = new SelectList(db.IngredientTypes, "Id", "Name", measureConversion.IngredientTypeId);
            return View(measureConversion);
        }

        // GET: MeasureConversions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeasureConversion measureConversion = await db.MeasureConversions.FindAsync(id);
            if (measureConversion == null)
            {
                return HttpNotFound();
            }
            return View(measureConversion);
        }

        // POST: MeasureConversions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MeasureConversion measureConversion = await db.MeasureConversions.FindAsync(id);
            db.MeasureConversions.Remove(measureConversion);
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
