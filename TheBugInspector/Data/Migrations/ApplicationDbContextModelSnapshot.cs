﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TheBugInspector.Data;

#nullable disable

namespace TheBugInspector.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationUserProject", b =>
                {
                    b.Property<string>("CompanyMembersId")
                        .HasColumnType("text");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("integer");

                    b.HasKey("CompanyMembersId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("ApplicationUserProject");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TheBugInspector.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ProfilePictureId")
                        .HasColumnType("uuid");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("ProfilePictureId");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TheBugInspector.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<Guid?>("ImageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("TheBugInspector.Models.FileUpload", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("TheBugInspector.Models.Invite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<Guid>("CompanyToken")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("InviteDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InviteeEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("InviteeFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("InviteeId")
                        .HasColumnType("text");

                    b.Property<string>("InviteeLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("InvitorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsValid")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("JoinDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("InviteeId");

                    b.HasIndex("InvitorId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("TheBugInspector.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TheBugInspector.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DeveloperUserId")
                        .HasColumnType("text");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsArchivedByProject")
                        .HasColumnType("boolean");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("SubmitterUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperUserId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("SubmitterUserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("TheBugInspector.Models.TicketAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TicketId")
                        .HasColumnType("integer");

                    b.Property<Guid?>("UploadId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.HasIndex("UploadId");

                    b.HasIndex("UserId");

                    b.ToTable("TicketAttachments");
                });

            modelBuilder.Entity("TheBugInspector.Models.TicketComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TicketId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.HasIndex("UserId");

                    b.ToTable("TicketComments");
                });

            modelBuilder.Entity("ApplicationUserProject", b =>
                {
                    b.HasOne("TheBugInspector.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("CompanyMembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TheBugInspector.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TheBugInspector.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TheBugInspector.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TheBugInspector.Data.ApplicationUser", b =>
                {
                    b.HasOne("TheBugInspector.Models.Company", "Company")
                        .WithMany("CompanyMembers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Models.FileUpload", "ProfilePicture")
                        .WithMany()
                        .HasForeignKey("ProfilePictureId");

                    b.Navigation("Company");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("TheBugInspector.Models.Company", b =>
                {
                    b.HasOne("TheBugInspector.Models.FileUpload", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("TheBugInspector.Models.Invite", b =>
                {
                    b.HasOne("TheBugInspector.Models.Company", "Company")
                        .WithMany("Invites")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Data.ApplicationUser", "Invitee")
                        .WithMany()
                        .HasForeignKey("InviteeId");

                    b.HasOne("TheBugInspector.Data.ApplicationUser", "Invitor")
                        .WithMany()
                        .HasForeignKey("InvitorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Invitee");

                    b.Navigation("Invitor");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TheBugInspector.Models.Project", b =>
                {
                    b.HasOne("TheBugInspector.Models.Company", "Company")
                        .WithMany("Projects")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("TheBugInspector.Models.Ticket", b =>
                {
                    b.HasOne("TheBugInspector.Data.ApplicationUser", "DeveloperUser")
                        .WithMany()
                        .HasForeignKey("DeveloperUserId");

                    b.HasOne("TheBugInspector.Models.Project", "Project")
                        .WithMany("Tickets")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Data.ApplicationUser", "SubmitterUser")
                        .WithMany()
                        .HasForeignKey("SubmitterUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeveloperUser");

                    b.Navigation("Project");

                    b.Navigation("SubmitterUser");
                });

            modelBuilder.Entity("TheBugInspector.Models.TicketAttachment", b =>
                {
                    b.HasOne("TheBugInspector.Models.Ticket", "Ticket")
                        .WithMany("Attachments")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Models.FileUpload", "Upload")
                        .WithMany()
                        .HasForeignKey("UploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Data.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");

                    b.Navigation("Upload");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TheBugInspector.Models.TicketComment", b =>
                {
                    b.HasOne("TheBugInspector.Models.Ticket", "Ticket")
                        .WithMany("Comments")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TheBugInspector.Data.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TheBugInspector.Models.Company", b =>
                {
                    b.Navigation("CompanyMembers");

                    b.Navigation("Invites");

                    b.Navigation("Projects");
                });

            modelBuilder.Entity("TheBugInspector.Models.Project", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("TheBugInspector.Models.Ticket", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
