using Microsoft.EntityFrameworkCore;
using ProfilesAPI.Models;
using System.Reflection.Emit;

namespace ProfilesAPI.Context
{
    public class ProfilesDbContext : DbContext
    {
        public ProfilesDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<Receptionist> Receptionists { get; set; } = null!;
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Patient>()
                .HasIndex(u => new { u.FirstName, u.LastName, u.MiddleName })
                .HasDatabaseName("PatientNameIndex");

            builder.Entity<Patient>()
                .HasOne(x => x.Account)
                .WithOne(x => x.Patient)
                .HasForeignKey<Patient>(x => x.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Doctor>()
                .HasOne(x => x.Account)
                .WithOne(x => x.Doctor)
                .HasForeignKey<Doctor>(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<Receptionist>()
                .HasOne(x => x.Account)
                .WithOne(x => x.Receptionist)
                .HasForeignKey<Receptionist>(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<Doctor>()
                .HasOne(x => x.DoctorSpecialization)
                .WithMany(x => x.Doctors)
                .HasForeignKey(x => x.SpecializationId);

            builder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Email = "yulia@mail.ru",
                    PasswordHash = "k��s�4��k�N�Z?WG���/\u001dI�\u001eRݷ�[K", //1
                    PhoneNumber = "+375333186223",
                    IsEmailVerified = true,
                    PhotoId = 1,
                    CreatedBy = "Julia",
                    CreatedAt = DateTime.Parse("2023-10-24 10:39:28.7234190")
                });
        }
    }
}