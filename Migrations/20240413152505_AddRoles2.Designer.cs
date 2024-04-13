﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnet_facebook.Models.Contexts;

#nullable disable

namespace dotnet_facebook.Migrations
{
    [DbContext(typeof(TestContext))]
    [Migration("20240413152505_AddRoles2")]
    partial class AddRoles2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Groups.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<DateTime>("GroupCreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupDescription")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("GroupPictureFileName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Groups.GroupUser", b =>
                {
                    b.Property<int>("GroupUserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupUserID"));

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("GroupRole")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GroupUserID");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeId"));

                    b.Property<DateTime>("LikeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LikeId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("OwnerUserUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("PostId");

                    b.HasIndex("GroupId");

                    b.HasIndex("OwnerUserUserId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("Post");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Post");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Roles.SiteRole", b =>
                {
                    b.Property<int>("SiteRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SiteRoleId"));

                    b.Property<string>("SiteRoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SiteRoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("TagId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PostId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.PrivateMessage", b =>
                {
                    b.Property<int>("PrivateMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrivateMessageId"));

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("MessageDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiverUserId")
                        .HasColumnType("int");

                    b.Property<int>("SenderUserId")
                        .HasColumnType("int");

                    b.HasKey("PrivateMessageId");

                    b.HasIndex("ReceiverUserId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("PrivateMessages");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("AccountCreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.UserProfile", b =>
                {
                    b.Property<int>("UserProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserProfileId"));

                    b.Property<string>("ProfilePictureFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserBio")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserProfileId");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("UserProfile");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Comment", b =>
                {
                    b.HasBaseType("dotnet_facebook.Models.DatabaseObjects.Posts.Post");

                    b.Property<int?>("PostId1")
                        .HasColumnType("int");

                    b.HasIndex("PostId1");

                    b.HasDiscriminator().HasValue("Comment");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.MainPost", b =>
                {
                    b.HasBaseType("dotnet_facebook.Models.DatabaseObjects.Posts.Post");

                    b.Property<int?>("ParentGroupGroupId")
                        .HasColumnType("int");

                    b.Property<int>("PostLatitude")
                        .HasColumnType("int");

                    b.Property<int>("PostLongitude")
                        .HasColumnType("int");

                    b.HasIndex("ParentGroupGroupId");

                    b.HasDiscriminator().HasValue("MainPost");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Groups.GroupUser", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Groups.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Like", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Posts.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Post", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Groups.Group", null)
                        .WithMany("GroupPosts")
                        .HasForeignKey("GroupId");

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "OwnerUser")
                        .WithMany()
                        .HasForeignKey("OwnerUserUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.UserProfile", null)
                        .WithMany("UserPosts")
                        .HasForeignKey("UserProfileId");

                    b.Navigation("OwnerUser");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Tag", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Groups.Group", null)
                        .WithMany("Tags")
                        .HasForeignKey("GroupId");

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Posts.Post", null)
                        .WithMany("Tags")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.PrivateMessage", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.UserProfile", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("dotnet_facebook.Models.DatabaseObjects.Users.UserProfile", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Comment", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Posts.Post", "ParentPost")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Posts.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId1");

                    b.Navigation("ParentPost");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.MainPost", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Groups.Group", "ParentGroup")
                        .WithMany()
                        .HasForeignKey("ParentGroupGroupId");

                    b.Navigation("ParentGroup");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Groups.Group", b =>
                {
                    b.Navigation("GroupPosts");

                    b.Navigation("Tags");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.User", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("UserProfile")
                        .IsRequired();
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.UserProfile", b =>
                {
                    b.Navigation("UserPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
