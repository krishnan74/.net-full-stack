using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Subject;

namespace QuizupAPI.Misc.Mappers
{
    public class SubjectMapper
    {
        public Subject? MapSubjectAddRequestSubject(SubjectDTO addRequestDto)
        {
            Subject subject = new();
            subject.Code = addRequestDto.Code;
            subject.Name = addRequestDto.Name;
            subject.CreatedAt = DateTime.UtcNow;
            subject.UpdatedAt = DateTime.UtcNow;
            return subject;

        }

        public Subject? MapSubjectUpdateRequestSubject(Subject existingSubject, SubjectDTO updateRequestDto)
        {
            existingSubject.Code = updateRequestDto.Code;
            existingSubject.Name = updateRequestDto.Name;
            existingSubject.UpdatedAt = DateTime.UtcNow;
            return existingSubject;
        }
    
    }
}