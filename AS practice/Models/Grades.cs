namespace AS_practice.Models
{
    public class Grades
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubjectName { get; set; }
        public int? GradeValue { get; set; }
        public int StudentId { get; set; }
        public int LecturerId { get; set; }
    }
}