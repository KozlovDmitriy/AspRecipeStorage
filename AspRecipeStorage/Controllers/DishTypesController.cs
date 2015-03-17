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
    [Authorize(Roles = "Admin")]
    public class DishTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DishTypes
        public async Task<ActionResult> Index()
        {
            return View(await db.DishType.ToListAsync());
        }

        // GET: DishTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishType dishType = await db.DishType.FindAsync(id);
            if (dishType == null)
            {
                return HttpNotFound();
            }
            return View(dishType);
        }

        // GET: DishTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DishTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] DishType dishType)
        {
            if (ModelState.IsValid)
            {
                db.DishType.Add(dishType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dishType);
        }

        // GET: DishTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishType dishType = await db.DishType.FindAsync(id);
            if (dishType == null)
            {
                return HttpNotFound();
            }
            return View(dishType);
        }

        // POST: DishTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] DishType dishType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dishType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dishType);
        }

        // GET: DishTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishType dishType = await db.DishType.FindAsync(id);
            if (dishType == null)
            {
                return HttpNotFound();
            }
            return View(dishType);
        }

        // POST: DishTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DishType dishType = await db.DishType.FindAsync(id);
            db.DishType.Remove(dishType);
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
