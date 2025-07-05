using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TraficViolation.Models;

public partial class TrafficViolationDbContext : DbContext
{
    public TrafficViolationDbContext()
    {
    }

    public TrafficViolationDbContext(DbContextOptions<TrafficViolationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Citizen> Citizens { get; set; }

    public virtual DbSet<ReportStatus> ReportStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<ViolationReport> ViolationReports { get; set; }

    public virtual DbSet<ViolationType> ViolationTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Citizen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__citizens__3213E83FC2406BD4");

            entity.ToTable("citizens");

            entity.HasIndex(e => e.Email, "UQ__citizens__AB6E6164B922CE59").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<ReportStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__report_s__3213E83FA81BC237");

            entity.ToTable("report_statuses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F89CE5620");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572C3DCDD2D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CitizenId).HasColumnName("citizen_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Citizen).WithMany(p => p.Users)
                .HasForeignKey(d => d.CitizenId)
                .HasConstraintName("FK__users__citizen_i__4CA06362");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__4D94879B");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_rol__3213E83FE951B7E7");

            entity.ToTable("user_roles");

            entity.HasIndex(e => e.RoleName, "UQ__user_rol__783254B188B79311").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicles__3213E83F8CEBDFD7");

            entity.ToTable("vehicles");

            entity.HasIndex(e => e.LicensePlate, "UQ__vehicles__F72CD56E1F117889").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(50)
                .HasColumnName("license_plate");
            entity.Property(e => e.Make)
                .HasMaxLength(100)
                .HasColumnName("make");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .HasColumnName("model");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Year).HasColumnName("year");

            entity.HasOne(d => d.Owner).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vehicles__owner___3B75D760");
        });

        modelBuilder.Entity<ViolationReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__violatio__3213E83F9B129F48");

            entity.ToTable("violation_reports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CitizenId).HasColumnName("citizen_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.ReportDate)
                .HasDefaultValueSql("(sysdatetimeoffset())")
                .HasColumnName("report_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            entity.Property(e => e.ViolationTypeId).HasColumnName("violation_type_id");

            entity.HasOne(d => d.Citizen).WithMany(p => p.ViolationReports)
                .HasForeignKey(d => d.CitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__violation__citiz__4316F928");

            entity.HasOne(d => d.Status).WithMany(p => p.ViolationReports)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__violation__statu__45F365D3");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.ViolationReports)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__violation__vehic__440B1D61");

            entity.HasOne(d => d.ViolationType).WithMany(p => p.ViolationReports)
                .HasForeignKey(d => d.ViolationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__violation__viola__44FF419A");
        });

        modelBuilder.Entity<ViolationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__violatio__3213E83FBDDEE1CB");

            entity.ToTable("violation_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
