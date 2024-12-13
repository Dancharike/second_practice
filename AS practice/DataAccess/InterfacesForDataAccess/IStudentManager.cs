using System.Collections.Generic;

namespace AS_practice.DataAccess.InterfacesForDataAccess
{
    public interface IStudentManager
    {
        List<int> ViewGrades(int studentId);
    }
}