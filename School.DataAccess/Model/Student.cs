using System.Collections.Generic;

namespace School.DataAccess.Model
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
 
        public string LastName { get; set; }

        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
     }
}
