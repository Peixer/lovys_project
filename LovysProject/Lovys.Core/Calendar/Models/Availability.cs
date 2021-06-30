using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lovys.Core.Calendar.Models
{
    public class Availability
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string StartTime { get; set; }        
        public string EndTime { get; set; } 
        public DayOfWeek DayOfWeek { get; set; }
        public User User { get; set; }
    }
}