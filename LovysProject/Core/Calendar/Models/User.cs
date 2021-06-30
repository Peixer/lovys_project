using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Calendar.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Candidate,
        Interviewer
    }
}