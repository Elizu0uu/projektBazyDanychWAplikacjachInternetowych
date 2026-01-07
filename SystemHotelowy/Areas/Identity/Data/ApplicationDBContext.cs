using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using SystemHotelowy.Areas.Identity.Data;
using SystemHotelowy.Models;

namespace SystemHotelowy.Areas.Identity.Data;

public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100); 
        builder.Property(x => x.LastName).HasMaxLength(100);

    }
    public DbSet<Rooms> Rooms { get; set; }
    public DbSet<Booking>? Bookings { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Status> Statutes {  get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "Receptionist", NormalizedName = "RECEPTIONIST" },
            new IdentityRole { Id = "3", Name = "Visitor", NormalizedName = "VISITOR" }
         );
        builder.Entity<RoomType>().HasData(
            new RoomType { Id = 1, Name = "Single", Description = "Room for one" },
            new RoomType { Id = 2, Name = "Double", Description = "Room for two" },
            new RoomType { Id = 3, Name = "MultiRoom", Description = "Room for group" }
         );
        builder.Entity<Status>().HasData(
            new Status { Id = 1, Name = "Pending" },
            new Status { Id = 2, Name = "Confirmed" },
            new Status { Id = 3, Name = "Check-in" },
            new Status { Id = 4, Name = "Cancelled" },
            new Status { Id = 5, Name = "Checked-out" },
            new Status { Id = 6, Name = "No-show" }
        );
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
}
