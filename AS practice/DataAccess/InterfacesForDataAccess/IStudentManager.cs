using System.Collections.Generic;
using AS_practice.Models;

namespace AS_practice.DataAccess.InterfacesForDataAccess
{
    public interface IStudentManager
    {
        List<StudentGrades> GetStudentData(int? studentId);
    }
}