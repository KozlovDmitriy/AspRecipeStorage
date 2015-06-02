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
            ViewBag.Amounts = new List<int?>();
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
            if (userId == null)
            {
                userId = User.Identity.GetUserId<int>();
            }
            string code = null;
            User user = db.Users.Find(userId);
            if (user != null)
            {
                code = user.ActivateCode;
            }
            if (code != null)
            {
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);
                ViewBag.WarningMessage = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">this link</a>";
            }
            this.IndexViewBagSetter();
            return View(recipeSet.OrderByDescending(i => i.Id));
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
                        j.Ingredients.Any(k => k.IngredientType.Name.Contains(word)) ||
                        j.StepInstruments.Any(k => k.Instrument.Name.Contains(word))
                    )
                );
        }

        private List<int> FindAllRecipeSteps(int id)
        {
            List<RecipeStep> steps = db.RecipeSteps.Where(i => i.RecipeId == id).ToList();
            List<int> result = steps.Select(i => i.Id).ToList();
            foreach( RecipeStep step in steps )
            {
                if (step.ChildRecipeId != null)
                {
                    result.AddRange(this.FindAllRecipeSteps(step.ChildRecipeId.Value));
                }
            }
            return result;
        }

        private void AddIngredientsSetToAdd(List<string> ingredientNames, List<int> measures, List<int?> amounts) 
        {
            if (
                ingredientNames != null &&
                ingredientNames.Count > 0 &&
                measures != null &&
                measures.Count > 0 &&
                amounts != null &&
                amounts.Count > 0 &&
                User != null && 
                User.Identity != null
            ) 
            {
                int userId = User.Identity.GetUserId<int>();
                if (userId != 0)
                {
                    DateTime dt = DateTime.Now;
                    IngredientsSet set = new IngredientsSet { 
                        UserId = userId,
                        DateCreate = dt,
                        Name = "Набор от " + dt.ToString("dd.MM.yy HH:mm"),
                        IngredientsSetRows = new List<IngredientsSetRow> ()
                    };
                    for (int i = 0; i < amounts.Count; ++i)
                    {
                        string ingredient = ingredientNames[i];
                        IngredientType itype = db.IngredientTypes.FirstOrDefault(j => j.Name == ingredient);
                        if (itype != null)
                        {
                            IngredientsSetRow row = new IngredientsSetRow
                            {
                                IngredientsSetId = set.Id,
                                Amount = amounts[i],
                                IngredientTypeId = itype.Id,
                                MeasureTypeId = measures[i]
                            };
                            set.IngredientsSetRows.Add(row);
                        }
                    }
                    if (set.IngredientsSetRows.Count > 0)
                    {
                        db.IngredientsSets.Add(set);
                        db.SaveChanges();
                    }
                }
            }
        }

        public ActionResult FilterIndex(
            string keywords = "", 
            List<string> ingredientNames = null,
            List<int> measures = null, 
            List<int?> amounts = null,
            List<int> dishTypeFilter = null, 
            int? userId = null,
            int? setId = null
        )
        {
            if (setId != null)
            {
                IngredientsSet set = db.IngredientsSets
                    .Include(i => i.IngredientsSetRows.Select(j => j.IngredientType))
                    .Include(i => i.IngredientsSetRows.Select(j => j.MeasureType))
                    .FirstOrDefault(i => i.Id == setId);
                if (set != null)
                {
                    amounts = set.IngredientsSetRows.Select(i => i.Amount).ToList();
                    measures = set.IngredientsSetRows.Select(i => i.MeasureTypeId).ToList();
                    ingredientNames = set.IngredientsSetRows.Select(i => i.IngredientType.Name).ToList();
                }
            }
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
                if (setId == null)
                {
                    this.AddIngredientsSetToAdd(ingredientNames, measures, amounts);
                }
                IQueryable<int> ingredientsIds = null;
                for (int m = 0; m < ingredientNames.Count; ++m)
                {
                    string name2 = ingredientNames[m].Clone() as string;
                    int measureId2 = measures[m];
                    int amount2 = amounts[m] == null ? 0 : amounts[m].Value;
                    IQueryable<int> ids = db.Ingredient.Include(i => i.IngredientType).Where(i =>
                        name2 == i.IngredientType.Name &&
                        measureId2 == i.MeasureTypeId &&
                        amount2 >= i.Amount
                    ).Select(i => i.Id);
                    ingredientsIds = ingredientsIds != null ? ingredientsIds.Concat(ids) : ids;
                    IQueryable<MeasureConversion> fmc = db.MeasureConversions
                        .Include(i => i.IngredientType.Ingredients)
                        .Where(i =>
                            i.IngredientType.Name == name2 &&
                            i.MeasureTypeFirstId == measureId2
                        );
                    IQueryable<int> ids2 = db.Ingredient.Include(i => i.IngredientType).Where(i =>
                       name2 == i.IngredientType.Name &&
                       fmc.Any(j => j.MeasureTypeSecondId == i.MeasureTypeId && amount2*j.Ratio >= i.Amount)
                    ).Select(i => i.Id);
                    ingredientsIds = ingredientsIds.Concat(ids2);
                    IQueryable<MeasureConversion> smc = db.MeasureConversions
                        .Include(i => i.IngredientType.Ingredients)
                        .Where(i =>
                            i.IngredientType.Name == name2 &&
                            i.MeasureTypeSecondId == measureId2
                        );
                    IQueryable<int> ids3 = db.Ingredient.Include(i => i.IngredientType).Where(i =>
                       name2 == i.IngredientType.Name &&
                       smc.Any(j => j.MeasureTypeFirstId == i.MeasureTypeId && amount2/j.Ratio >= i.Amount)
                    ).Select(i => i.Id);
                    ingredientsIds = ingredientsIds.Concat(ids3).Distinct();
                }
                IQueryable<int> recipeStepsIds = db.RecipeSteps.Where(i => i.Ingredients.All(j => ingredientsIds.Contains(j.Id)) || i.ChildRecipeId != null).Select(i => i.Id);
                recipes = recipes.Where(i => i.RecipeSteps.All(j => recipeStepsIds.Contains(j.Id)));
                List<Recipe> recipesList = recipes.ToList();
                List<Recipe> recurseRecipes = new List<Recipe>();
                foreach (Recipe r in recipesList)
                {
                    List<int> allSteps = FindAllRecipeSteps(r.Id);
                    if (allSteps.All(i => recipeStepsIds.Contains(i)))
                    {
                        recurseRecipes.Add(r);
                    }
                }
                recipes = recurseRecipes.AsQueryable();
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
            ViewBag.IngredientNames = ingredientNames != null ? ingredientNames : new List<string>();
            ViewBag.Measures = measures != null ? measures : new List<int>();
            ViewBag.Amounts = amounts != null ? amounts : new List<int?>();
            ViewBag.Keywords = keywords != null ? keywords : "";
            ViewBag.DishTypes = db.DishType.Select(i => new CheckBoxItem
            {
                Id = i.Id,
                Name = i.Name,
                Selected = dtFilter.Contains(i.Id)
            }).ToList();
            recipes = recipes.Where(i => dtFilter.Contains(i.DishTypeId));
            return View("Index", recipes.OrderByDescending(i => i.Id));
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

        private string ReadFile(HttpPostedFileBase picture) 
        {
            int userId = User.Identity.GetUserId<int>();
            string name = userId.ToString() + DateTime.Now.Ticks.ToString();
            if (picture != null && userId != 0)
            {
                name = name + picture.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Images"), name);
                // file is uploaded
                picture.SaveAs(path);
            }
            return name;
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

        private void AttachInstrumentToDB(Instrument instrument)
        {
            string name = instrument.Name;
            if (db.Instruments.Where(i => i.Name == name).Count() > 0)
            {
                instrument.Id = db.Instruments.AsNoTracking().Where(i => i.Name == name).FirstOrDefault().Id;
                db.Instruments.Attach(instrument);
            }
            else
            {
                instrument.ForAllUsers = false;
                db.Instruments.Add(instrument);
            }
        }

        private void AttachRecipeStepToDB(RecipeStep recipeStep)
        {
            int rsid = recipeStep.Id;
            List<Ingredient> ingredients = recipeStep.Ingredients.ToList<Ingredient>();
            List<int> ids = ingredients.Select(i => i.Id).ToList();
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

            List<StepInstrument> instruments = recipeStep.StepInstruments.ToList<StepInstrument>();
            List<int> ids2 = instruments.Select(i => i.Id).ToList();
            db.StepInstruments.RemoveRange(db.StepInstruments.Where(i => i.RecipeStep_Id == rsid && !ids2.Contains(i.Id)));
            for (int j = 0; j < instruments.Count; ++j)
            {
                this.AttachInstrumentToDB(instruments[j].Instrument);
                instruments[j].Instrument_Id = instruments[j].Instrument.Id;
                instruments[j].RecipeStep_Id = recipeStep.Id;
                if (instruments[j].Id == 0)
                {
                    db.StepInstruments.Add(instruments[j]);
                }
                else
                {
                    db.Entry(instruments[j]).State = EntityState.Modified;
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
            if (ModelState.IsValid && recipe.Description != null)
            {
                recipe.AuthorId = User.Identity.GetUserId<int>();
                if (recipePicture != null) { 
                    recipe.Picture = new Picture { Name = this.ReadFile(recipePicture) };
                }
                int time = 0;
                List<RecipeStep> steps = recipe.RecipeSteps.ToList<RecipeStep>();
                for (int i = 0; i < steps.Count; ++i)
                {
                    if (steps[i].ChildRecipeId != null)
                    {
                        int childId = steps[i].ChildRecipeId.Value;
                        steps[i].Time = db.Recipe.AsNoTracking().FirstOrDefault(j => j.Id == childId).Time;
                    }
                    time += steps[i].Time;
                    steps[i].StepNumber = i + 1;
                    stepPictures[i].ForEach(j =>
                    {
                        if (j != null)
                        {
                            steps[i].Pictures.Add(new Picture { Name = this.ReadFile(j) });
                        }
                    });
                    this.AttachRecipeStepToDB(steps[i]);
                }
                recipe.Time = time;
                db.Recipe.Add(recipe);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.WarningMessage = "Операция отклонена, т.к. не все необходимые поля были заполнены!";
            ViewBag.MeasureTypes = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name", recipe.DishTypeId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName", recipe.AuthorId);
            return View(recipe);
        }

        private bool CheckId(int curId, int? id)
        {
            if (id != null)
            {
                if (curId == id)
                {
                    return false;
                }
                else
                {
                    Recipe recipe = db.Recipe.Include(i => i.RecipeSteps).FirstOrDefault(i => i.Id == curId);
                    foreach (RecipeStep rs in recipe.RecipeSteps)
                    {
                        if (rs.ChildRecipeId != null && !CheckId(rs.ChildRecipeId.Value, id))
                        {
                            return false;
                        }

                    }
                }
            }
            return true;
        }

        private List<int> GetHierarchyRecipeList(int? id)
        {
            List<int> list = db.Recipe.Select(i => i.Id).ToList();
            List<int> result = new List<int>();
            for (int i = 0; i < list.Count; ++i)
            {
                if (CheckId(list[i], id))
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        public ActionResult RecipeStepHierarchy(int? id)
        {
            List<int> list = this.GetHierarchyRecipeList(id);
            ViewBag.Recipes = new SelectList(db.Recipe.Where(i => list.Contains(i.Id)),"Id", "Name");
            return PartialView("_RecipeStepHierarchy", new RecipeStep());
        }

        public ActionResult RecipeStep()
        {
            return PartialView("_RecipeStepCreate", new RecipeStep());
        }

        public ActionResult Ingredient()
        {
            return PartialView("_IngredientCreate", new Ingredient { IngredientType = new IngredientType(), MeasureType = new MeasureType() });
        }

        public ActionResult Instrument()
        {
            return PartialView("_InstrumentCreate", new StepInstrument { Instrument = new Instrument() });
        }

        public SelectList GetMeasureTypes(string ingredientName) 
        {
            List<MeasureType> mt = null;
            if (ingredientName != null)
            {
                ViewBag.IsEmpty = !db.IngredientTypes.Select(i => i.Name).Contains(ingredientName);
                if (!ViewBag.IsEmpty)
                { 
                    mt = db.MeasureTypes.Where(i => 
                        i.Ingredients.Select(j => j.IngredientType.Name).Contains(ingredientName)
                    ).Distinct().ToList();
                    List<int> mtIds = mt.Select(i => i.Id).ToList();
                    IQueryable<MeasureConversion> mcs = db.MeasureConversions
                        .Include(i => i.IngredientType)
                        .Include(i => i.MeasureFirstType)
                        .Include(i => i.MeasureSecondType)
                        .Where(i => i.IngredientType.Name == ingredientName && (mtIds.Contains(i.MeasureTypeFirstId) || mtIds.Contains(i.MeasureTypeSecondId)));
                    mt.AddRange(mcs.Select(i => i.MeasureFirstType).ToList());
                    mt.AddRange(mcs.Select(i => i.MeasureSecondType).ToList());
                    mt = mt.Distinct().ToList();
                }
            }
            else
            {
                ViewBag.IsEmpty = true;
            }
            return new SelectList(mt, "Id", "Name");
        }

        public ActionResult IngredientFilteritem(string ingredientName)
        {
            SelectList measureTypes = this.GetMeasureTypes(ingredientName);
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
                .Include(r => r.RecipeSteps.Select(i => i.ChildRecipe))
                .Include(r => r.RecipeSteps.Select(i => i.Pictures))
                .Include(r => r.RecipeSteps.Select(i => i.StepInstruments.Select(g => g.Instrument)))
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.MeasureType)))
                .Include(r => r.RecipeSteps.Select(i => i.Ingredients.Select(g => g.IngredientType)))
                .SingleOrDefaultAsync(r => r.Id == id.Value);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            List<int> list = this.GetHierarchyRecipeList(id);
            ViewBag.Recipes = list.Count > 0 ? new SelectList(db.Recipe.Where(i => list.Contains(i.Id)), "Id", "Name") : null;
            ViewBag.MeasureTypes = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName");
            return View(recipe);
        }

        private void DeleteImageFile(string name)
        {
            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Images"), name));
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
            if (ModelState.IsValid)
            {
                int rid = recipe.Id;
                int time = 0;
                var steps = recipe.RecipeSteps;
                List<int> rsids = recipe.RecipeSteps.Select(i => i.Id).ToList();
                db.RecipeSteps.RemoveRange(db.RecipeSteps.Where(i => i.RecipeId == rid && !rsids.Contains(i.Id)));
                if (recipePicture != null && recipePicture.ContentLength > 0)
                {
                    Picture picture = db.Pictures.FirstOrDefault(i => i.Id == recipe.PictureId);
                    if (picture != null)
                    {
                        this.DeleteImageFile(picture.Name);
                        db.Pictures.Remove(picture);
                        recipe.PictureId = null;
                    }
                    Picture newpicture = new Picture { Name = this.ReadFile(recipePicture) };
                    db.Pictures.Add(newpicture);
                    recipe.Picture = newpicture;
                }
                else
                {
                    recipe.Picture = null;
                }
                for (int i = 0; i < steps.Count; ++i)
                {
                    int stepNumber = i + 1;
                    int rsid = steps.ElementAt(i).Id;
                    AttachRecipeStepToDB(steps.ElementAt(i));
                    steps.ElementAt(i).RecipeId = rid;
                    steps.ElementAt(i).StepNumber = stepNumber;
                    if (pictureStepStatuses[i] > 0 && stepPictures[i][0] == null)
                    {
                        steps.ElementAt(i).Pictures = null;
                    }
                    else
                    {
                        db.Pictures.AsNoTracking()
                            .Where(j => j.RecipeSteps.Any(l => l.RecipeId == rid && l.StepNumber == stepNumber))
                            .ToList().ForEach(j => this.DeleteImageFile(j.Name));
                        db.Pictures.RemoveRange(
                            db.Pictures.Where(j => j.RecipeSteps.Any(l => l.RecipeId == rid && l.StepNumber == stepNumber))
                        );
                    }
                    stepPictures[i].ForEach(j =>
                    {
                        if (j != null)
                        {
                            Picture img = new Picture { Name = this.ReadFile(j) };
                            db.Pictures.Add(img);
                            steps.ElementAt(i).Pictures.Add(img);
                        }
                    });
                    if (steps.ElementAt(i).ChildRecipeId != null)
                    {
                        int childId = steps.ElementAt(i).ChildRecipeId.Value;
                        steps.ElementAt(i).Time = db.Recipe.AsNoTracking().FirstOrDefault(j => j.Id == childId).Time;
                    }
                    time += steps.ElementAt(i).Time;
                    var entry = db.Entry(steps.ElementAt(i));
                    db.Entry(steps.ElementAt(i)).State = rsid == 0 ? EntityState.Added : EntityState.Modified;
                }
                recipe.Time = time;
                db.Entry(recipe).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            List<int> list = this.GetHierarchyRecipeList(recipe.Id);
            ViewBag.Recipes = list.Count > 0 ? new SelectList(db.Recipe.Where(i => list.Contains(i.Id)), "Id", "Name") : null;
            ViewBag.MeasureTypes = new SelectList(db.MeasureTypes, "Id", "Name");
            ViewBag.DishTypeId = new SelectList(db.DishType, "Id", "Name");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "UserName");
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

        private void DeleteParentSteps(int id)
        {
            List<Recipe> recipes = db.Recipe.Include(i => i.RecipeSteps).Where(i => i.RecipeSteps.Any(j => j.ChildRecipeId == id)).ToList();
            int number = 1;
            foreach (Recipe recipe in recipes)
            {
                for (int i = 0; i < recipe.RecipeSteps.Count; ++i)
                {
                    if (recipe.RecipeSteps.ElementAt(i).ChildRecipeId == id)
                    {
                        db.RecipeSteps.Remove(recipe.RecipeSteps.ElementAt(i));
                    }
                    else
                    {
                        recipe.RecipeSteps.ElementAt(i).StepNumber = number;
                        db.Entry(recipe.RecipeSteps.ElementAt(i)).State = EntityState.Modified;
                        ++number;
                    }
                }
            }
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
            db.Pictures.AsNoTracking()
                .Where(i => i.Recipes.Any(j => j.Id == id) || i.RecipeSteps.Any(j => j.RecipeId == id))
                .ToList().ForEach(i => this.DeleteImageFile(i.Name));
            this.DeleteParentSteps(recipe.Id);
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

        public JsonResult AutoCompleteInstrument(string term)
        {
            var result = (from r in db.Instruments
                          where r.Name.ToLower().Contains(term.ToLower()) && r.ForAllUsers
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
