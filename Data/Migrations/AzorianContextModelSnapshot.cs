using System;
using Azorian.Data;
using Azorian.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Azorian.Data.Migrations;

[DbContext(typeof(AzorianContext))]
partial class AzorianContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("Azorian.Models.User", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("text");

            b.HasKey("Id");

            b.ToTable("Users");
        });

        modelBuilder.Entity("Azorian.Models.Role", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            b.Property<string>("Description")
                .HasColumnType("text");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("text");

            b.HasKey("Id");

            b.HasIndex("Name")
                .IsUnique();

            b.ToTable("Roles");
        });

        modelBuilder.Entity("Azorian.Models.AuditLog", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            b.Property<string>("Action")
                .IsRequired()
                .HasColumnType("text");

            b.Property<int?>("PerformedByUserId")
                .HasColumnType("integer");

            b.Property<int?>("RoleId")
                .HasColumnType("integer");

            b.Property<int?>("TargetUserId")
                .HasColumnType("integer");

            b.Property<DateTime>("Timestamp")
                .HasColumnType("timestamp with time zone");

            b.HasKey("Id");

            b.ToTable("AuditLogs");
        });

        modelBuilder.Entity("Azorian.Models.UserRole", b =>
        {
            b.Property<int>("UserId")
                .HasColumnType("integer");

            b.Property<int>("RoleId")
                .HasColumnType("integer");

            b.HasKey("UserId", "RoleId");

            b.HasIndex("RoleId");

            b.ToTable("UserRoles");

            b.HasOne("Azorian.Models.Role", "Role")
                .WithMany("UserRoles")
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("Azorian.Models.User", "User")
                .WithMany("UserRoles")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("Azorian.Models.Role", b =>
        {
            b.Navigation("UserRoles");
        });

        modelBuilder.Entity("Azorian.Models.User", b =>
        {
            b.Navigation("UserRoles");
        });
#pragma warning restore 612, 618
    }
}
