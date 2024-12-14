using System.Collections.Generic;

namespace AS_practice.DataAccess.InterfacesForDataAccess
{
    public interface IAdminManager
    {
        void AddUser(string username, string password, string role); // add lecturers and students or even another admin
        void DeleteUser(int userId); // delete users except admin
        void CreateGroup(string groupName);
        void CreateCourse(string courseName);
        void AddStudentToGroup(int studentId, int groupId);
        void AssignGroupToCourse(int groupId, int studentId); // it is better to assign students group to course, rather than assign student and group separately to course
        void AssignSubjectsToCourse(int courseId, List<int> subjectIds); // can add up to 7 subjects
        void AssignLecturerToCourse(int lecturerId, int courseId);
        void DeleteGroup(int groupId);
        void DeleteCourse(int courseId);
        void CreateSubject(string subjectName);
        void DeleteSubject(int subjectId);
    }
}