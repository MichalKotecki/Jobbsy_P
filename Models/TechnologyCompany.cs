using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jobbsy.Models
{
    public class TechnologyCompany
    {
        [Key]
        public int technologyCompanyID { get; set; }
        public int technologyID { get; set; }
        public int companyID { get; set; }

        public Technology Technology { get; set; }

        public Company Company { get; set; }
        
       
    }
}

