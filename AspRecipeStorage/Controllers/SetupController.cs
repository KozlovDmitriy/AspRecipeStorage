using AspRecipeStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspRecipeStorage.Controllers
{
    public class SetupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            db.Seed();
            return Content("success");
        }
    }
}