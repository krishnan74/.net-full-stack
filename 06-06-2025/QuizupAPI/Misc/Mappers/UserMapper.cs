using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models.DTOs.Student;

namespace QuizupAPI.Misc.Mappers
{
    public class UserMapper
    {
        public User? MapTeacherAddRequestUser(TeacherAddRequestDTO addRequestDto)
        {
            User user = new();
            user.Username = addRequestDto.Email; 
            user.Role = "Teacher";

            return user;
        }

        public User? MapStudentAddRequestUser(StudentAddRequestDTO addRequestDto)
        {
            User user = new();
            user.Username = addRequestDto.Email;
            user.Role = "Student";

            return user;
        }
    }
}