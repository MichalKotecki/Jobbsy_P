using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Jobbsy.Models
{
    public class Company
    {
        [Key]
        public int companyID { get; set; }
        public String companyName { get; set; }
        public String description { get; set; }
        public double locationX { get; set; }
        public double locationY { get; set; }
        public String website { get; set; }

        // FOREIGN KEY One-To-Many relation of Company and Photo
        public ICollection<Photo> PhotoCollection { get; set; }
        // FOREIGN KEY Many-To-Many relation of Technology and Company
        public ICollection<TechnologyCompany> TechnologyCompany { get; set; }
        // FOREIGN KEY One-To-Many relation of Company and Photo
        public ICollection<Comment> CommentCollection { get; set; }

    }
}
