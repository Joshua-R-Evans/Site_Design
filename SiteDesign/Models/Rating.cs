//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiteDesign.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rating
    {
        public int RatingId { get; set; }
        public string UserId { get; set; }
        public int TemplateId { get; set; }
        public Nullable<int> Stars { get; set; }
        public string Comments { get; set; }
    
        public virtual Template Template { get; set; }
        public virtual SiteUser SiteUser { get; set; }
    }
}