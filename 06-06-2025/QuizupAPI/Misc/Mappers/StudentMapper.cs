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

        public Student? MapStudentUpdateRequestStudent(Student existingStudent, StudentUpdateRequestDTO updateRequestDto)
        {
            existingStudent.FirstName = updateRequestDto.FirstName;
            existingStudent.LastName = updateRequestDto.LastName;
            existingStudent.Class = updateRequestDto.Class;

            return existingStudent;
        }
    
    }
}