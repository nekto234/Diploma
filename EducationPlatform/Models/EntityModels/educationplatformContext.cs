using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EducationPlatform.Models.EntityModels
{
    public partial class EducationPlatformContext : DbContext
    {
        public EducationPlatformContext()
        {
        }

        public EducationPlatformContext(DbContextOptions<EducationPlatformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Chat> Chat { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseModule> CourseModule { get; set; }
        public virtual DbSet<CourseStudent> CourseStudent { get; set; }
        public virtual DbSet<Mark> Mark { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleFile> ModuleFile { get; set; }
        public virtual DbSet<OnlineList> OnlineList { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<TwoFactorUser> TwoFactorUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=educationplatform;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.SecurityStamp)
                    .IsRequired()
                    .HasDefaultValueSql("(N'f1f7f444-fb7b-4469-9979-0720852f2300')");

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK__Comments__C3B4DFCA05CACD68");

                entity.HasIndex(e => e.MarkId);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Mark)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.MarkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Comments__MarkId__70DDC3D8");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.SubjectId);

                entity.HasIndex(e => e.TeacherId);

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TeacherId).IsRequired();

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_SubjectId");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Course)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Course_Teacher_AspNetUsers");
            });

            modelBuilder.Entity<CourseModule>(entity =>
            {
                entity.HasIndex(e => e.CourseId);

                entity.HasIndex(e => e.ModuleId);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseModule)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CourseModule_Course");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.CourseModule)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CourseModule_Module");
            });

            modelBuilder.Entity<CourseStudent>(entity =>
            {
                entity.HasIndex(e => e.CourseId);

                entity.HasIndex(e => e.StudentId);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseStudent)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("fk_CourseStudent_Course");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.CourseStudent)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("fk_CourseStudent_Student");
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasIndex(e => e.CourseModuleId);

                entity.HasIndex(e => e.StudentId);

                entity.HasOne(d => d.CourseModule)
                    .WithMany(p => p.Mark)
                    .HasForeignKey(d => d.CourseModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Mark_CourseModule");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Mark)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Mark_Student");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasIndex(e => e.SubjectId);

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Module)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("fk_Module_Subject");
            });

            modelBuilder.Entity<ModuleFile>(entity =>
            {
                entity.HasIndex(e => e.ModuleId);

                entity.Property(e => e.FileUrl)
                    .IsRequired()
                    .HasMaxLength(800);

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.ModuleFile)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ModuleFil__Modul__73BA3083");
            });

            modelBuilder.Entity<OnlineList>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("UQ__Student__1788CC4D8BA74B7F")
                    .IsUnique();

                entity.Property(e => e.Faculty).HasMaxLength(50);

                entity.Property(e => e.HasAccess).HasDefaultValueSql("((0))");

                entity.Property(e => e.Skills).HasMaxLength(500);

                entity.Property(e => e.University).HasMaxLength(50);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Student_AspNetUsers");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<TwoFactorUser>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("UQ__TwoFacto__1788CC4D25DB7886")
                    .IsUnique();

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.TwoFactorUser)
                    .HasForeignKey<TwoFactorUser>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TwoFactor__UserI__32767D0B");
            });
        }
    }
}
