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
        public ApplicationDbContext db = new ApplicationDbContext();

        private void IndexViewBagSetter() 
        {
            ViewBag.DishTypes = db.DishType.Select(i => new CheckBoxItem
            {
                Id = i.Id,
                Name = i.Name,
                Selected = true
            }).ToList();
        }

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
            this.IndexViewBagSetter();
            return View(recipeSet.OrderBy(i => i.Id));
        }

        private IQueryable<Recipe> SearchRecipesBykeyword(string word)
        {
            return db.Recipe
                .Include(i => i.DishType)
                .Include(i => i.RecipeSteps.Select(j => j.Ingredients.Select(k => k.IngredientType)))
                .Where(i =>
                    i.Name.Contains(word) ||
                    i.Description.Contains(word) ||
                    i.RecipeSteps.Any(j =>
                        j.Discription.Contains(word) ||
                        j.Ingredients.Any(k => k.IngredientType.Name.Contains(word))
                    )
                );
        }

        public ActionResult FilterIndex(
            string keywords = "", 
            List<string> ingredientNames = null,
            List<int> measures = null, 
            List<int?> amounts = null,
            List<int> dishTypeFilter = null, 
            int? userId = null
        )
        {
            List<string> keywordsList = keywords.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            IQueryable<Recipe> recipes = null;
            if (keywordsList.Count == 0)
            {
                recipes = db.Recipe.Include(i => i.DishType).AsQueryable();
            }
            else
            {
                foreach (string word in keywordsList)
                {
                    recipes = recipes == null ?
                        SearchRecipesBykeyword(word.Clone() as string) :
                        recipes.Union(
                            SearchRecipesBykeyword(word.Clone() as string)
                        );
                }
            }
            recipes.Distinct();
            if (ingredientNames != null)
            {
                string name = ingredientNames[0].Clone() as string;
                int measureId = measures[0];
                int amount = amounts[0] == null ? 0 : amounts[0].Value;
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
                    ingredientsIds = ingredientsIds.Concat(ids);
                }
                IQueryable<int> recipeStepsIds = db.RecipeSteps.Where(i => i.Ingredients.All(j => ingredientsIds.Contains(j.Id))).Select(i => i.Id);
                recipes = recipes.Where(i => i.RecipeSteps.All(j => recipeStepsIds.Contains(j.Id)));
            }
            if (userId != null)
            {
                if (userId == -1)
                {
                    userId = User.Identity.GetUserId<int>();
                }
                recipes = recipes.Where(i => i.User.Id == userId);
                ViewBag.UserId = userId;
            }
            List<int> dtFilter = dishTypeFilter == null ? new List<int>() : dishTypeFilter;
            ViewBag.DishTypes = db.DishType.Select(i => new CheckBoxItem
            {
                Id = i.Id,
                Name = i.Name,
                Selected = dtFilter.Contains(i.Id)
            }).ToList();
            recipes = recipes.Where(i => dtFilter.Contains(i.DishTypeId));
            return View("Index", recipes.OrderBy(i => i.Id));
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
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.MeasureType)))
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.IngredientType)))
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
                ingredientType.Id = db.IngredientTypes.AsNoTracking().Where(it => it.Name == name).FirstOrDefault().Id;
                db.IngredientTypes.Attach(ingredientType);
            }
            else
            {
                db.IngredientTypes.Add(ingredientType);
            }
        }

        private void AttachRecipeStepToDB(RecipeStep recipeStep)
        {
            List<Ingredient> ingredients = recipeStep.Ingredients.ToList<Ingredient>();
            List<int> ids = ingredients.Select(i => i.Id).ToList();
            int rsid = recipeStep.Id;
            db.Ingredient.RemoveRange(db.Ingredient.Where(i => i.RecipeStepId == rsid && !ids.Contains(i.Id)));
            for (int j = 0; j < ingredients.Count; ++j)
            {

                this.AttachIngredientTypeToDB(ingredients[j].IngredientType);
                ingredients[j].IngredientTypeId = ingredients[j].IngredientType.Id;
                ingredients[j].RecipeStepId = recipeStep.Id;
                if (ingredients[j].Id == 0)
                {
                    db.Ingredient.Add(ingredients[j]);
                }
                else 
                {
                    db.Entry(ingredients[j]).State = EntityState.Modified;
                }
            }
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Create(Recipe recipe, HttpPostedFileBase recipePicture, List<List<HttpPostedFileBase>> stepPictures)
        {
            if (ModelState.IsValid)
            {
                recipe.AuthorId = User.Identity.GetUserId<int>();
                if (recipePicture != null) { 
                    recipe.Picture = new Picture { Data = this.ReadFile(recipePicture) };
                }
                int time = 0;
                List<RecipeStep> steps = recipe.RecipeSteps.ToList<RecipeStep>();
                for (int i = 0; i < steps.Count; ++i)
                {
                    time += steps[i].Time;
                    steps[i].StepNumber = i + 1;
                    stepPictures[i].ForEach(j =>
                    {
                        if (j != null)
                        {
                            steps[i].Pictures.Add(new Picture { Data = this.ReadFile(j) });
                        }
                    });
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
            return PartialView("_RecipeStepCreate", new RecipeStep());
        }

        public ActionResult Ingredient()
        {
            return PartialView("_IngredientCreate", new Ingredient { IngredientType = new IngredientType(), MeasureType = new MeasureType() });
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
            Recipe recipe = await db.Recipe
                .Include(r => r.Picture)
                .Include(r => r.DishType)
                .Include(r => r.User)
                .Include(r => r.RecipeSteps.Select(i => i.Pictures))
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.MeasureType)))
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.IngredientType)))
                .SingleOrDefaultAsync(r => r.Id == id.Value);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            ViewBag.MeasureTypes = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName");
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> Edit(
            Recipe recipe, 
            HttpPostedFileBase recipePicture, 
            List<List<HttpPostedFileBase>> stepPictures,
            List<int> pictureStepStatuses
        )
        {
            int rid = recipe.Id;
            int time = 0;
            var steps = recipe.RecipeSteps;
            List<int> rsids = recipe.RecipeSteps.Select(i => i.Id).ToList();
            db.RecipeSteps.RemoveRange(db.RecipeSteps.Where(i => i.RecipeId == rid && !rsids.Contains(i.Id)));
            if (recipePicture != null && recipePicture.ContentLength > 0)
            {
                db.Pictures.Remove(db.Pictures.AsNoTracking().FirstOrDefault(i => i.Id == recipe.PictureId));
                recipe.Picture = new Picture {Data = this.ReadFile(recipePicture)};
            }
            else
            {
                recipe.Picture = null;
            }
            for (int i = 0; i < steps.Count; ++i)
            {
                int stepNumber = i + 1;
                int rsid = steps.ElementAt(i).Id ;
                AttachRecipeStepToDB(steps.ElementAt(i));
                steps.ElementAt(i).RecipeId = rid;
                steps.ElementAt(i).StepNumber = stepNumber;
                if (pictureStepStatuses[i] > 0 && stepPictures[i][0] == null)
                {
                    steps.ElementAt(i).Pictures = null;
                }
                else
                {
                    db.Pictures.RemoveRange(
                        db.Pictures.Where(j => j.RecipeSteps.Any(l => l.RecipeId == rid && l.StepNumber == stepNumber))
                    );
                }
                stepPictures[i].ForEach(j =>
                {
                    if (j != null)
                    {
                        Picture img = new Picture { Data = this.ReadFile(j) };
                        db.Pictures.Add(img);
                        steps.ElementAt(i).Pictures.Add(img);
                    }
                });
                time += steps.ElementAt(i).Time;
                var entry = db.Entry(steps.ElementAt(i));
                db.Entry(steps.ElementAt(i)).State = rsid == 0 ? EntityState.Added : EntityState.Modified;
            }
            recipe.Time = time;
            db.Entry(recipe).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
            Recipe recipe = await db.Recipe
                .Include(r => r.DishType)
                .Include(r => r.User)
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.MeasureType)))
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.IngredientType)))
                .SingleOrDefaultAsync(r => r.Id == id);
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
