using QuizupAPI.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasOne(s => s.User)
                                        .WithOne(u => u.Student)
                                        .HasForeignKey<Student>(s => s.Email)
                                        .HasConstraintName("FK_User_Student")
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

        }
    }
}