using Microsoft.EntityFrameworkCore;
using OverloadControl.Models;

namespace OverloadControl.DataAccessor
{
    public class OCDbContext : DbContext
    {
        private void InitDB()
        {
            using (var db = new OCDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        //private string connectionString = "Server=.;Database=OverloadControl1;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
        private string connectionString = "Server=localhost;Port=3306;Database=OverloadControl1;Uid=root;Pwd=123456;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Case>(entity =>
            {
                entity.ToTable("ZC_Case");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                //entity.HasOne(c => c.Violators).WithMany(c => c.Cases).HasForeignKey(c => c.VId);
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("ZC_Category");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Law>(entity =>
            {
                entity.ToTable("ZC_Law");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<LawType>(entity =>
            {
                entity.ToTable("ZC_LawType");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Police>(entity =>
            {
                entity.ToTable("ZC_Police");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Progress>(entity =>
            {
                entity.ToTable("ZC_Progress");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ZC_Role");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Violators>(entity =>
            {
                entity.ToTable("ZC_Violators");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Case_Progress>(entity =>
            {
                entity.ToTable("ZC_Case_Progress");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasOne(c => c.Case).WithMany(c => c.Case_Progresses).HasForeignKey(c => c.CaseId);
                entity.HasOne(c => c.Progress).WithMany(c => c.Case_Progresses).HasForeignKey(c => c.ProgressId);
            });
            modelBuilder.Entity<Law_Case>(entity =>
            {
                entity.ToTable("ZC_Law_Case");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasOne(c => c.Case).WithMany(c => c.law_Cases).HasForeignKey(c => c.CaseId);
                entity.HasOne(c => c.Law).WithMany(c => c.law_Cases).HasForeignKey(c => c.LawId);
            });
            modelBuilder.Entity<Police_Case>(entity =>
            {
                entity.ToTable("ZC_Police_Case");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasOne(c => c.Case).WithMany(c => c.Police_Cases).HasForeignKey(c => c.CaseId);
                entity.HasOne(c => c.Police).WithMany(c => c.Police_Cases).HasForeignKey(c => c.PoliceId);
            });
            modelBuilder.Entity<Police_Role>(entity =>
            {
                entity.ToTable("ZC_Police_Role");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasOne(c => c.Police).WithMany(c => c.Police_Roles).HasForeignKey(c => c.PoliceId);
                entity.HasOne(c => c.Role).WithMany(c => c.Police_Roles).HasForeignKey(c => c.RoleId);
            });

            modelBuilder.Entity<Police_Category>(entity =>
            {
                entity.ToTable("ZC_Police_Category");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasOne(c => c.category).WithMany(c => c.police_Categories).HasForeignKey(c => c.CategoryId);
                entity.HasOne(c => c.police).WithMany(c => c.police_Categories).HasForeignKey(c => c.PoliceId);
            });
            modelBuilder.Entity<Law_LawType>(entity =>
            {
                entity.ToTable("ZC_Law_LawType");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedOnAdd();
                entity.HasOne(c => c.law).WithMany(c => c.law_LawTypes).HasForeignKey(c => c.LawId);
                entity.HasOne(c => c.lawType).WithMany(c => c.law_LawTypes).HasForeignKey(c => c.LawTypeId);
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Case> Cases { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Law> Laws { get; set; }
        public DbSet<Police> Polices { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Violators> Violators { get; set; }

        public DbSet<Case_Progress> Case_Progresses { get; set; }

        public DbSet<Police_Case> Police_Cases { get; set; }

        public DbSet<Police_Role> Police_Roles { get; set; }
        public DbSet<Police_Category> police_Categories { get; set; }
        public DbSet<Law_Case> law_Cases { get; set; }

        public DbSet<Law_LawType> law_LawTypes { get; set; }
        public DbSet<LawType> lawTypes { get; set; }
    }
}