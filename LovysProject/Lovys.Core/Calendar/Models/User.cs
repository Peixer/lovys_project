using System.ComponentModel.DataAnnotations.Schema;

namespace Lovys.Core.Calendar.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

        public bool ShouldSerializePassword()
        {
            return false;
        }
    }

    public enum UserRole
    {
        Candidate = 0,
        Interviewer = 1
    }
}