using System;

namespace Core.Calendar.Models
{
    public class Availability
    {
        public string StartTime { get; set; }        
        public string EndTime { get; set; } 
        public DayOfWeek DayOfWeek { get; set; }
        public User User { get; set; }
    }
}