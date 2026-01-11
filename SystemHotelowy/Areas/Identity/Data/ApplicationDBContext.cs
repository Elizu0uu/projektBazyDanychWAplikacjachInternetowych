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

        var hasher = new PasswordHasher<AppUser>();

        var admin = new AppUser
        {
            Id = "a1", 
            UserName = "admin@hotel.pl",
            NormalizedUserName = "ADMIN@HOTEL.PL",
            Email = "admin@hotel.pl",
            NormalizedEmail = "ADMIN@HOTEL.PL",
            EmailConfirmed = true,
            FirstName = "Adam",
            LastName = "Admin"
        };
        admin.PasswordHash = hasher.HashPassword(admin, "Administracja123!");

        var recep = new AppUser
        {
            Id = "a2",
            UserName = "recepcja@hotel.pl",
            NormalizedUserName = "RECEPCJA@HOTEL.PL",
            Email = "recepcja@hotel.pl",
            NormalizedEmail = "RECEPCJA@HOTEL.PL",
            EmailConfirmed = true,
            FirstName = "Rafał",
            LastName = "Recepcja"
        };
        recep.PasswordHash = hasher.HashPassword(recep, "Recepcja123!");
        var gosc = new AppUser
        {
            Id = "a3",
            UserName = "gosc@hotel.pl",
            NormalizedUserName = "GOSC@HOTEL.PL",
            Email = "gosc@hotel.pl",
            NormalizedEmail = "GOSC@HOTEL.PL",
            EmailConfirmed = true,
            FirstName = "Gabriela",
            LastName = "Gosc"
        };
        gosc.PasswordHash = hasher.HashPassword(gosc, "Visitor123!");

        builder.Entity<AppUser>().HasData(admin, recep, gosc);

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = "a1", RoleId = "1" }, 
            new IdentityUserRole<string> { UserId = "a2", RoleId = "2" },
            new IdentityUserRole<string> { UserId = "a3", RoleId = "3"}
        );
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
}
