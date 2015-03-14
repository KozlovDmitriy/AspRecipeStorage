using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspRecipeStorage.Models
{
    public partial class ApplicationDbContext 
    {
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}