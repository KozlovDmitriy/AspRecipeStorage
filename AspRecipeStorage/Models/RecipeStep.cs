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
    
    public partial class RecipeStep
    {
        public RecipeStep()
        {
            this.Ingredients = new HashSet<Ingredient>();
            this.Pictures = new HashSet<Picture>();
            this.StepInstruments = new HashSet<StepInstrument>();
        }
    
        public int Id { get; set; }
        public string Discription { get; set; }
        public int Time { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
    
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual ICollection<StepInstrument> StepInstruments { get; set; }
    }
}
