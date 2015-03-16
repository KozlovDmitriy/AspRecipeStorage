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
using Microsoft.AspNet.Identity;
using System.IO;

namespace AspRecipeStorage.Controllers
{
    public class RecipesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipes
        public async Task<ActionResult> Index()
        {
            var recipeSet = db.Recipe.Include(r => r.DishType).Include(r => r.User);
            return View(await recipeSet.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = await db.Recipe
                .Include(r => r.DishType)
                .Include(r => r.User)
                .Include(r => r.RecipeStep.Select(i => i.Ingredients))
                .SingleOrDefaultAsync(r => r.Id == id.Value);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,DishTypeId")] Recipe recipe, HttpPostedFileBase pictureinput)
        {
            if (ModelState.IsValid)
            {
                recipe.AuthorId = User.Identity.GetUserId<int>();
                if (pictureinput != null && pictureinput.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(pictureinput.InputStream))
                    {
                        recipe.Picture = reader.ReadBytes(pictureinput.ContentLength);
                    }
                }
                db.Recipe.Add(recipe);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name", recipe.DishTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName", recipe.AuthorId);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = await db.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name", recipe.DishTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName", recipe.AuthorId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Picture,DishTypeId,AuthorId")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipe).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name", recipe.DishTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName", recipe.AuthorId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = await db.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Recipe recipe = await db.Recipe.FindAsync(id);
            db.Recipe.Remove(recipe);
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
