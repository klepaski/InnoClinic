using Microsoft.EntityFrameworkCore;
using AppointmentsAPI.Models;

namespace AppointmentsAPI.Context
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<Result> Results { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Result>()
                .HasOne(x => x.Appointment)
                .WithOne(x => x.Result)
                .HasForeignKey<Result>(x => x.AppointmentId);
        }
    }
}
