namespace AS_practice.DataAccess.InterfacesForDataAccess
{
    public interface ILecturerManager
    {
        void AddGrade(int studentId, int subjectId, int grade);
        void EditGrade(int studentId, int subjectId, int grade);
    }
}