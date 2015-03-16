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
    public class RecipeStepsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RecipeSteps
        public async Task<ActionResult> Index()
        {
            var recipeStep = db.RecipeStep.Include(r => r.Recipe);
            return View(await recipeStep.ToListAsync());
        }

        // GET: RecipeSteps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipeStep recipeStep = await db.RecipeStep.FindAsync(id);
            if (recipeStep == null)
            {
                return HttpNotFound();
            }
            return View(recipeStep);
        }

        // GET: RecipeSteps/Create
        public ActionResult Create()
        {
            ViewBag.RecipeId = new SelectList(db.Recipe, "Id", "Name");
            return View();
        }

        // POST: RecipeSteps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Discription,Time,StepNumber,RecipeId")] RecipeStep recipeStep)
        {
            if (ModelState.IsValid)
            {
                db.RecipeStep.Add(recipeStep);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.RecipeId = new SelectList(db.Recipe, "Id", "Name", recipeStep.RecipeId);
            return View(recipeStep);
        }

        // GET: RecipeSteps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipeStep recipeStep = await db.RecipeStep.FindAsync(id);
            if (recipeStep == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecipeId = new SelectList(db.Recipe, "Id", "Name", recipeStep.RecipeId);
            return View(recipeStep);
        }

        // POST: RecipeSteps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Discription,Time,StepNumber,RecipeId")] RecipeStep recipeStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipeStep).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.RecipeId = new SelectList(db.Recipe, "Id", "Name", recipeStep.RecipeId);
            return View(recipeStep);
        }

        // GET: RecipeSteps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipeStep recipeStep = await db.RecipeStep.FindAsync(id);
            if (recipeStep == null)
            {
                return HttpNotFound();
            }
            return View(recipeStep);
        }

        // POST: RecipeSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RecipeStep recipeStep = await db.RecipeStep.FindAsync(id);
            db.RecipeStep.Remove(recipeStep);
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
