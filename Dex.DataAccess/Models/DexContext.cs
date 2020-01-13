using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dex.DataAccess.Models
{
    public partial class DexContext : IdentityDbContext<AspNetUsers, AspNetRoles, string, AspNetUserClaims,
        AspNetUserRoles, AspNetUserLogins, AspNetRoleClaims, AspNetUserTokens>
    {
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<ProjectFavorites> ProjectFavorites { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<AspNetUsersSession> AspNetUsersSession { get; set; }

        public DexContext(DbContextOptions<DexContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
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

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new {e.LoginProvider, e.ProviderKey});

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new {e.UserId, e.RoleId});

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.UserId).HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new {e.UserId, e.LoginProvider, e.Name});

                entity.Property(e => e.UserId).HasMaxLength(100);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

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

                entity.Property(e => e.Id).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Message).HasMaxLength(500);

                entity.Property(e => e.StackTrace).HasMaxLength(1000);
            });

            modelBuilder.Entity<ProjectFavorites>(entity =>
            {
                entity.HasKey(e => new {e.UserId, e.ProjectId});

                entity.Property(e => e.UserId)
                    .HasColumnName("User_Id")
                    .HasMaxLength(100);

                entity.Property(e => e.ProjectId).HasColumnName("Project_Id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectFavorites)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectFavorites_Projects");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectFavorites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectFavorites_AspNetUsers");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(512);

                entity.Property(e => e.ExecutableName).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RepositoryLink).HasMaxLength(512);
            });

            modelBuilder.Entity<AspNetUsersSession>(entity =>
            {
                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.LogoutDate).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUsersSession)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspNetUsersSession_AspNetUsers");
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
