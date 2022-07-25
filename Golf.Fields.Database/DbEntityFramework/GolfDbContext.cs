using Microsoft.EntityFrameworkCore;
using Golf.Fields.Shared;

namespace Golf.Fields.Database
{
    public partial class GolfDbContext : DbContext
    {

        public virtual DbSet<User>? Users { get; set; }

        public virtual DbSet<DatabaseInfo>? DatabaseInfo { get; set; }

        public virtual DbSet<City>? Cities { get; set; }

        public virtual DbSet<Country>? Countries { get; set; }

        public virtual DbSet<Field>? Fields { get; set; }

        public virtual DbSet<Reservation>? Reservations { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var sqlConnString = AppSettingsJson.GetSqlServerConnString();

                if (string.IsNullOrWhiteSpace(sqlConnString))
                    throw new ArgumentNullException(nameof(sqlConnString));

                optionsBuilder.UseSqlServer(sqlConnString);
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Phone)
                    .HasMaxLength(60)
                    .IsRequired(true);
            });


            modelBuilder.Entity<DatabaseInfo>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .IsRequired(true);

                entity.Property(e => e.CreatedOn)
                    .IsRequired(true);

                entity.Property(e => e.ModifiedOn)
                    .IsRequired(true);
            });


            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsRequired(true);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_City_Country");
            });


            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired(true);
            });


            modelBuilder.Entity<Field>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Fields)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Field_City");
            });


            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.HasOne(d => d.Field)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.FieldId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Reservation_Field");
            });
        }
    }
}

