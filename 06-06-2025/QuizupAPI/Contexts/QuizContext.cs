using QuizupAPI.Models;
using QuizupAPI.Interfaces;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models.DTOs.Student;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Contexts
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; } = null!;
        public DbSet<Quiz> quizzes { get; set; } = null!;
        public DbSet<Question> questions { get; set; } = null!;
        public DbSet<QuizQuestion> quizQuestions { get; set; } = null!;
        public DbSet<Answer> answers { get; set; } = null!;
        public DbSet<QuizSubmission> quizSubmissions { get; set; } = null!;
        public DbSet<Teacher> teachers { get; set; } = null!;
        public DbSet<Student> students { get; set; } = null!;

        public DbSet<Classe> classes { get; set; } = null!;
        public DbSet<Subject> subjects { get; set; } = null!;
        public DbSet<TeacherSubject> teacherSubjects { get; set; } = null!;
        public DbSet<TeacherClass> teacherClasses { get; set; } = null!;

        public DbSet<ClassSubject> classSubjects { get; set; } = null!;

        public DbSet<TeacherSummaryDTO> teacherSummary { get; set; }
        public DbSet<StudentSummaryDTO> studentSummary { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is ISoftDeletable && e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                ((ISoftDeletable)entry.Entity).IsDeleted = true;
                entry.State = EntityState.Modified;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Teacher>().HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<Student>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Quiz>().HasQueryFilter(q => !q.IsDeleted);

            modelBuilder.Entity<Student>().HasOne(s => s.User)
                                        .WithOne(u => u.Student)
                                        .HasForeignKey<Student>(s => s.Email)
                                        .HasConstraintName("FK_User_Student")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>().HasOne(s => s.Classe)
                                        .WithMany(c => c.Students)
                                        .HasForeignKey(s => s.ClassId)
                                        .HasConstraintName("FK_Student_Class")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Teacher>().HasOne(t => t.User)
                                        .WithOne(u => u.Teacher)
                                        .HasForeignKey<Teacher>(t => t.Email)
                                        .HasConstraintName("FK_User_Teacher")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quiz>().HasOne(q => q.Teacher)
                                        .WithMany(t => t.Quizzes)
                                        .HasForeignKey(q => q.TeacherId)
                                        .HasConstraintName("FK_Quiz_Teacher")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quiz>().HasOne(q => q.Classe)
                                        .WithMany(c => c.Quizzes)
                                        .HasForeignKey(q => q.ClassId)
                                        .HasConstraintName("FK_Quiz_Class")
                                        .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Quiz>().HasOne(q => q.Subject)
                                        .WithMany(s => s.Quizzes)
                                        .HasForeignKey(q => q.SubjectId)
                                        .HasConstraintName("FK_Quiz_Subject")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuizQuestion>().HasOne(qq => qq.Quiz)
                                                .WithMany(q => q.QuizQuestions)
                                                .HasForeignKey(q => q.QuizId)
                                                .HasConstraintName("FK_QuizQuestion_Quiz")
                                                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuizQuestion>().HasOne(qq => qq.Question)
                                                .WithMany(qs => qs.QuizQuestions)
                                                .HasForeignKey(qs => qs.QuestionId)
                                                .HasConstraintName("FK_QuizQuestion_Question")
                                                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuizSubmission>().HasOne(qsub => qsub.Quiz)
                                        .WithMany(q => q.Submissions)
                                        .HasForeignKey(qsub => qsub.QuizId)
                                        .HasConstraintName("FK_QuizSubmission_Quiz")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuizSubmission>().HasOne(qsub => qsub.Student)
                                        .WithMany(s => s.QuizSubmissions)
                                        .HasForeignKey(qsub => qsub.StudentId)
                                        .HasConstraintName("FK_QuizSubmission_Student")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Answer>().HasOne(a => a.Question)
                                        .WithMany(q => q.Answers)
                                        .HasForeignKey(a => a.QuestionId)
                                        .HasConstraintName("FK_Answer_Question")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Answer>().HasOne(a => a.QuizSubmission)
                                        .WithMany(qsub => qsub.Answers)
                                        .HasForeignKey(a => a.QuizSubmissionId)
                                        .HasConstraintName("FK_Answer_QuizSubmission")
                                        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<TeacherSubject>().HasOne(ts => ts.Teacher)
                                        .WithMany(t => t.TeacherSubjects)
                                        .HasForeignKey(ts => ts.TeacherId)
                                        .HasConstraintName("FK_TeacherSubject_Teacher")
                                        .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<TeacherSubject>().HasOne(ts => ts.Subject)
                                        .WithMany(s => s.TeacherSubjects)
                                        .HasForeignKey(ts => ts.SubjectId)
                                        .HasConstraintName("FK_TeacherSubject_Subject")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherClass>().HasOne(ts => ts.Classe)
                                        .WithMany(c => c.Teachers)
                                        .HasForeignKey(ts => ts.ClassId)
                                        .HasConstraintName("FK_TeacherClass_Class")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherClass>().HasOne(ts => ts.Teacher)
                                        .WithMany(t => t.TeacherClasses)
                                        .HasForeignKey(ts => ts.TeacherId)      
                                        .HasConstraintName("FK_TeacherClass_Teacher")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSubject>().HasOne(cs => cs.Classe)
                                        .WithMany(c => c.ClassSubjects)
                                        .HasForeignKey(cs => cs.ClassId)
                                        .HasConstraintName("FK_ClassSubject_Class")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSubject>().HasOne(cs => cs.Subject)
                                        .WithMany(s => s.ClassSubjects)
                                        .HasForeignKey(cs => cs.SubjectId)
                                        .HasConstraintName("FK_ClassSubject_Subject")
                                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeacherClass>().HasOne(ts => ts.Teacher)
                                        .WithMany(t => t.TeacherClasses)
                                        .HasForeignKey(ts => ts.TeacherId)      
                                        .HasConstraintName("FK_TeacherClass_Teacher")
                                        .OnDelete(DeleteBehavior.Restrict);




            modelBuilder.Entity<TeacherSummaryDTO>().HasNoKey();
            modelBuilder.Entity<StudentSummaryDTO>().HasNoKey();
            
        }
    }
}