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
    
    public partial class MeasureType
    {
        public MeasureType()
        {
            this.Ingredients = new HashSet<Ingredient>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
