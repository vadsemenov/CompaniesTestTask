﻿namespace School.DataAccess.Model
{
    public class Assessment
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public int StudentId { get; set; }

        public ushort Mark { get; set; }
    }
}
