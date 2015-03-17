using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspRecipeStorage.Models
{
    public partial class Recipe
    {
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