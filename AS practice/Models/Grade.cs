namespace AS_practice.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int LecturerCourseId { get; set; }
        public int GradeValue { get; set; }
    }
}