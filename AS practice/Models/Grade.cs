using System;

namespace AS_practice.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public string SubjectName { get; set; }
        public int CategoryId { get; set; }
        public int GradeValue { get; set; }
    }
}