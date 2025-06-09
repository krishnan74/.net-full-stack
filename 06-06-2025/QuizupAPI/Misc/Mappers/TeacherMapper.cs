using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;

namespace QuizupAPI.Misc.Mappers
{
    public class TeacherMapper
    {
        public Teacher? MapTeacherAddRequestTeacher(TeacherAddRequestDTO addRequestDto)
        {
            Teacher teacher = new();
            teacher.FirstName = addRequestDto.FirstName;
            teacher.LastName = addRequestDto.LastName;
            teacher.Email = addRequestDto.Email;
            teacher.Subject = addRequestDto.Subject;

            return teacher;
        }

        public Teacher? MapTeacherUpdateRequestTeacher(TeacherUpdateRequestDTO updateRequestDto)
        {
            Teacher teacher = new();
            teacher.FirstName = updateRequestDto.FirstName;
            teacher.LastName = updateRequestDto.LastName;
            teacher.Subject = updateRequestDto.Subject;

            return teacher;
        }
    }
}