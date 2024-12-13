using System.Collections.Generic;

namespace AS_practice.DataAccess.InterfacesForDataAccess
{
    public interface IAdminManager
    {
        void AddUser(string username, string password, int roleId, int roleSpecificId); // add lecturers and students or even another admin
        void DeleteUser(int userId); // delete users except admin
        void CreateGroup(string groupName); //
        void CreateCourse(string courseName); //
        void AddStudentToGroup(int studentId, int groupId); //
        void AddStudentToCourse(int studentId, int courseId);
        void AssignSubjectsToGroup(int groupId, List<int> subjectIds); // can add up to 7 subjects
        void AssignLecturerToCourse(int lecturerId, int courseId); //
    }
}