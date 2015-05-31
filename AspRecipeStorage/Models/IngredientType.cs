//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AspRecipeStorage.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class IngredientType
    {
        public IngredientType()
        {
            this.Ingredients = new HashSet<Ingredient>();
            this.MeasureConversions = new HashSet<MeasureConversion>();
            this.IngredientsSetRows = new HashSet<IngredientsSetRow>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<MeasureConversion> MeasureConversions { get; set; }
        public virtual ICollection<IngredientsSetRow> IngredientsSetRows { get; set; }
    }
}
