using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Jobbsy.Models
{
    public class Technology
    {
        [Key]
        public int technologyID { get; set; }

        public string name { get; set; }
        public string description { get; set; }

        // FOREIGN KEY Many-To-Many relation of Technology and Company
        public ICollection <TechnologyCompany> TechnologyCompany { get; set; }
    }
}
