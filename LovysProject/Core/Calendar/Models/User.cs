namespace Core.Calendar.Models
{
    public class User
    {
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