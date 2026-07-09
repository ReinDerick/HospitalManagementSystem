using HospitalManagementSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Data
{
    public class HospitalDbContext : DbContext 
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
        {
        }
        
        public DbSet<HMRole> hMRoles { get; set; }

        public DbSet<HMUser> hMUsers { get; set; }

        public DbSet<HMUserRole> hMUserRoles { get; set; }

        public DbSet<Patient> patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HMUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID);

            modelBuilder.Entity<HMUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID);

            modelBuilder.Entity<Patient>()
                .HasOne(ur => ur.Doctor)
                .WithMany(p => p.DoctorPatient)
                .HasForeignKey(ur => ur.DoctorID);

            modelBuilder.Entity<Patient>()
                .HasOne(ur => ur.Nurse)
                .WithMany(p => p.NursePatient)
                .HasForeignKey(ur => ur.NurseID);
        }

    }
}
 