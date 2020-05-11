using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteDesign.Models
{   
    [MetadataType(typeof(TemplateMetadata))]
    public partial class Template
    {
        public const string UploadLocation = @"~/Content/UserContent";
        public double averageRating 
        { get
            {
                if (Ratings.Count() == 0)
                {
                    return 2.5;
                }
                return Ratings.Sum(r => r.Stars ?? 0) / (double)Ratings.Count( r => r.Stars.HasValue);
            } 
        }
    }
    public class TemplateMetadata
    {
        [Display(Name = "Name")]
        public string FolderName { get; set; }
    }
}