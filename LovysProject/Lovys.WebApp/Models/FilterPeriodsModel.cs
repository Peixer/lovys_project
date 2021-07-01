using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lovys.WebApp.Models
{
    public class FilterPeriodsModel
    {
        [Required] public List<string> Interviewers { get; set; }        
        [Required] public DateTime StartDate { get; set; }    
        [Required] public DateTime EndDate { get; set; }

    }
}