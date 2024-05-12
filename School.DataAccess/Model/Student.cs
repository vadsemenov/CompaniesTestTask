using System.Collections.Generic;

namespace School.DataAccess.Model
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        // public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
     }
}
