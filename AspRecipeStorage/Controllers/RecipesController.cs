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
        public ActionResult Index(int? userId = null)
        {
            var recipeSet = db.Recipe.Include(r => r.DishType).Include(r => r.User);
            if (userId != null) 
            {
                if (userId == -1) 
                {
                    userId = User.Identity.GetUserId<int>();
                }
                recipeSet = recipeSet.Where(i => i.User.Id == userId);
                ViewBag.UserId = userId;
            }
            ViewBag.DishTypes = db.DishType.Select(i => new CheckBoxItem { 
                Id = i.Id,
                Name = i.Name,
                Selected = true
            }).ToList();
            return View(recipeSet.OrderBy(i => i.Id));
        }

        public ActionResult FilterIndex(List<int> dishTypeFilter = null, int? userId = null)
        {
            var recipeSet = db.Recipe.Include(r => r.DishType).Include(r => r.User);
            if (userId != null)
            {
                if (userId == -1)
                {
                    userId = User.Identity.GetUserId<int>();
                }
                recipeSet = recipeSet.Where(i => i.User.Id == userId);
                ViewBag.UserId = userId;
            }
            List<int> dtFilter = dishTypeFilter == null ? new List<int>() : dishTypeFilter;
            ViewBag.DishTypes = db.DishType.Select(i => new CheckBoxItem
            {
                Id = i.Id,
                Name = i.Name,
                Selected = dtFilter.Contains(i.Id)
            }).ToList();
            recipeSet = recipeSet.Where(i => dtFilter.Contains(i.DishTypeId));
            return View("Index",recipeSet.OrderBy(i => i.Id));
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
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            ViewBag.MeasureTypes = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        private byte[] ReadFile(HttpPostedFileBase picture) 
        {
            byte[] result = null;
            if (picture != null && picture.ContentLength > 0)
            {
                using (var reader = new System.IO.BinaryReader(picture.InputStream))
                {
                    result = reader.ReadBytes(picture.ContentLength);
                }
            }
            return result;
        }

        private void AttachIngredientTypeToDB(IngredientType ingredientType) 
        {
            string name = ingredientType.Name;
            if (db.IngredientTypes.Where(it => it.Name == name).Count() > 0)
            {
                ingredientType.Id = db.IngredientTypes.Where(it => it.Name == name).FirstOrDefault().Id;
                db.IngredientTypes.Attach(ingredientType);
            }
        }

        private void AttachRecipeStepToDB(RecipeStep recipeStep)
        {
            List<Ingredient> ingredients = recipeStep.Ingredients.ToList<Ingredient>();
            for (int j = 0; j < ingredients.Count; ++j)
            {
                this.AttachIngredientTypeToDB(ingredients[j].IngredientType);
            }
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Create(Recipe recipe, HttpPostedFileBase recipePicture, List<HttpPostedFileBase> stepPictures)
        {
            if (ModelState.IsValid)
            {
                recipe.AuthorId = User.Identity.GetUserId<int>();
                recipe.Picture = this.ReadFile(recipePicture);
                int time = 0;
                List<RecipeStep> steps = recipe.RecipeStep.ToList<RecipeStep>();
                for (int i = 0; i < steps.Count; ++i)
                {
                    time += steps[i].Time;
                    steps[i].StepNumber = i + 1;
                    steps[i].Picture = this.ReadFile(stepPictures[i]);
                    this.AttachRecipeStepToDB(steps[i]);
                }
                recipe.Time = time;
                db.Recipe.Add(recipe);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MeasureTypes = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name", recipe.DishTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName", recipe.AuthorId);
            return View(recipe);
        }

        public ActionResult RecipeStep()
        {
            return PartialView("_RecipeStepCreate");
        }

        public ActionResult Ingredient()
        {
            return PartialView("_IngredientCreate", new SelectList(db.MeasureTypes, "Id", "Name"));
        }

        public ActionResult IngredientFilteritem(string ingredientName)
        {
            SelectList measureTypes = null;
            List<MeasureType> mt = null;
            if (ingredientName != null)
            {
                mt = db.MeasureTypes.Where(i => i.Ingredients
                    .Select(j => j.IngredientType.Name)
                    .Contains(ingredientName)
                ).Distinct().ToList();
                ViewBag.IsEmpty = !db.IngredientTypes.Select(i => i.Name).Contains(ingredientName);
            }
            else
            {
                ViewBag.IsEmpty = true;
            }
            measureTypes = new SelectList(mt, "Id", "Name");
            ViewBag.IngredientName = ingredientName == null ? "" : ingredientName;
            return PartialView("_IngredientFilteritem", measureTypes);
        }

        // GET: Recipes/Edit/5
        [Authorize(Roles = "Admin, User")]
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
        [Authorize(Roles = "Admin, User")]
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
        [Authorize(Roles = "Admin, User")]
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
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Recipe recipe = await db.Recipe.FindAsync(id);
            db.Recipe.Remove(recipe);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult AutoCompleteIngredient(string term)
        {
            var result = (from r in db.IngredientTypes
                          where r.Name.ToLower().Contains(term.ToLower())
                          select new { r.Id, r.Name }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        class IngredientMin
        {
            public string Name = "";
            public int MeasureId = 0;
            public int Amount = 0;
        }

        public ActionResult SearchRecipesByIngredients(List<string> ingredientNames = null, List<int> measures = null, List<int?> amounts = null) 
        {
            string name = ingredientNames[0].Clone() as string;
            int measureId = measures[0];
            int amount = amounts[0] == null ? 0 : amounts[0].Value;
            List<IngredientMin> ingredients = new List<IngredientMin>();            
            //IQueryable<int> ingredientTypesIds = db.IngredientTypes.Where(i => ingredientNames.Contains(i.Name) ).Select(i => i.Id);            
            IQueryable<int> ingredientsIds = db.Ingredient.Include(i => i.IngredientType).Where(i => 
                name == i.IngredientType.Name &&
                measureId == i.MeasureTypeId &&
                amount >= i.Amount
            ).Select(i => i.Id);
            for (int m = 1; m < ingredientNames.Count; ++m)
            {
                string name2 = ingredientNames[m].Clone() as string;
                int measureId2 = measures[m];
                int amount2 = amounts[m] == null ? 0 : amounts[m].Value;
                IQueryable<int> ids = db.Ingredient.Include(i => i.IngredientType).Where(i =>
                    name2 == i.IngredientType.Name &&
                    measureId2 == i.MeasureTypeId &&
                    amount2 >= i.Amount
                ).Select(i => i.Id);
                ingredientsIds = ingredientsIds.Concat( ids );
            }
            IQueryable<int> recipeStepsIds = db.RecipeStep.Where(i => i.Ingredients.All(j => ingredientsIds.Contains(j.Id))).Select(i => i.Id);
            IQueryable<Recipe> recipes = db.Recipe.Where(i => i.RecipeStep.All(j => recipeStepsIds.Contains(j.Id)));
            ViewBag.DishTypes = db.DishType.Select(i => new CheckBoxItem
            {
                Id = i.Id,
                Name = i.Name,
                Selected = true
            }).ToList();
            return View("Index", recipes.OrderBy(i => i.Id));
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
