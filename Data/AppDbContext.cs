using Microsoft.EntityFrameworkCore;

namespace InvigilatorSchedulerStandard.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<ExamSession> ExamSessions => Set<ExamSession>();
    public DbSet<TeacherRestrictedExamSession> TeacherRestrictedExamSessions => Set<TeacherRestrictedExamSession>();
    public DbSet<RuleConfig> RuleConfigs => Set<RuleConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeacherRestrictedExamSession>()
            .HasKey(x => new { x.TeacherId, x.ExamSessionId });

        modelBuilder.Entity<TeacherRestrictedExamSession>()
            .HasOne(x => x.Teacher)
            .WithMany(t => t.RestrictedExamSessions)
            .HasForeignKey(x => x.TeacherId);

        modelBuilder.Entity<TeacherRestrictedExamSession>()
            .HasOne(x => x.ExamSession)
            .WithMany()
            .HasForeignKey(x => x.ExamSessionId);

        // Ensure Unicode columns (SQL Server uses NVARCHAR for string by default in EF Core)
        // Seed default rules
        modelBuilder.Entity<RuleConfig>().HasData(
            new RuleConfig
            {
                Id = 1,
                Code = RuleCodes.Settings,
                JsonValue = "{\"defaultInvigilatorsPerExam\":2,\"backupInvigilatorsPerDay\":1,\"randomSeed\":5,\"fairnessEnabled\":true}"
            },
            new RuleConfig
            {
                Id = 2,
                Code = RuleCodes.NoExamForBackupSameDay,
                JsonValue = "{\"enabled\":true}"
            },
            new RuleConfig
            {
                Id = 3,
                Code = RuleCodes.MaxSessionsPerTeacherPerDay,
                JsonValue = "{\"value\":2}"
            }
        );
    }
}
