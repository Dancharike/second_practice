namespace AS_practice.DataAccess.InterfacesForDataAccess
{
    public interface ILecturerManager
    {
        void AddGrade(int studentId, int lecturerCourseId, int categoryId, int gradeValue);
        void EditGrade(int gradeId, int gradeValue);
    }
}