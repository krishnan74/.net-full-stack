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

            return teacher;
        }

        public Teacher? MapTeacherUpdateRequestTeacher(Teacher existingTeacher, TeacherUpdateRequestDTO updateRequestDto)
        {
            existingTeacher.FirstName = updateRequestDto.FirstName;
            existingTeacher.LastName = updateRequestDto.LastName;
            existingTeacher.UpdatedAt = DateTime.UtcNow;
            return existingTeacher;
        }
    }
}