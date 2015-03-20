using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspRecipeStorage.Models
{
    public partial class Recipe
    {
        [NotMapped] 
        public int Time
        {
            get
            {
                if (this != null)
                {
                    return this.RecipeStep.Sum(s => s.Time);
                }
                return 0;
            }
        }
    }
}