using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;

namespace QuizupAPI.Misc.Mappers
{
    public class StudentMapper
    {
        public Student? MapStudentAddRequestStudent(StudentAddRequestDTO addRequestDto)
        {
            Student student = new();
            student.FirstName = addRequestDto.FirstName;
            student.LastName = addRequestDto.LastName;
            student.Email = addRequestDto.Email;
            student.Class = addRequestDto.Class;

            return student;
        }

        public Student? MapStudentUpdateRequestStudent(StudentUpdateRequestDTO updateRequestDto)
        {
            Student student = new();
            student.FirstName = updateRequestDto.FirstName;
            student.LastName = updateRequestDto.LastName;
            student.Class = updateRequestDto.Class;

            return student;
        }
    }
}