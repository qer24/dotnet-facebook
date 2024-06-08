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
    [Migration("20240608150701_GroupTagsChange")]
    partial class GroupTagsChange
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GroupTag", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("int");

                    b.HasKey("GroupId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("GroupTag", (string)null);
                });

            modelBuilder.Entity("MainPostTag", b =>
                {
                    b.Property<int>("MainPostPostId")
                        .HasColumnType("int");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("int");

                    b.HasKey("MainPostPostId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("MainPostTag", (string)null);
                });

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

                    b.ToTable("GroupUsers");
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

                    b.Property<bool>("AdministrativePerms")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("SiteRoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SiteRoleId");

                    b.ToTable("SiteRoles");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Roles.UserSiteRole", b =>
                {
                    b.Property<int>("UserSiteRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserSiteRoleId"));

                    b.Property<int>("RoleSiteRoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserSiteRoleId");

                    b.HasIndex("RoleSiteRoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSiteRoles");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.Friendship", b =>
                {
                    b.Property<int>("FriendshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FriendshipId"));

                    b.Property<DateTime>("FriendshipDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("User1Id")
                        .HasColumnType("int");

                    b.Property<int>("User2Id")
                        .HasColumnType("int");

                    b.HasKey("FriendshipId");

                    b.HasIndex("User1Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.PrivateMessage", b =>
                {
                    b.Property<int>("PrivateMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrivateMessageId"));

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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

                    b.Property<string>("HashedPassword")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

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

                    b.Property<int>("ParentPostPostId")
                        .HasColumnType("int");

                    b.HasIndex("ParentPostPostId");

                    b.HasDiscriminator().HasValue("Comment");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.MainPost", b =>
                {
                    b.HasBaseType("dotnet_facebook.Models.DatabaseObjects.Posts.Post");

                    b.Property<int?>("ParentGroupGroupId")
                        .HasColumnType("int");

                    b.Property<string>("PostLocation")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("ParentGroupGroupId");

                    b.HasDiscriminator().HasValue("MainPost");
                });

            modelBuilder.Entity("GroupTag", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MainPostTag", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Posts.MainPost", null)
                        .WithMany()
                        .HasForeignKey("MainPostPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Groups.GroupUser", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Groups.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "User")
                        .WithMany()
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

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Roles.UserSiteRole", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Roles.SiteRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleSiteRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.Friendship", b =>
                {
                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "User1")
                        .WithMany()
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Users.User", "User2")
                        .WithMany()
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User1");

                    b.Navigation("User2");
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
                        .HasForeignKey("ParentPostPostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("dotnet_facebook.Models.DatabaseObjects.Posts.MainPost", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

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

                    b.Navigation("Users");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.Post", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.User", b =>
                {
                    b.Navigation("UserProfile")
                        .IsRequired();
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Users.UserProfile", b =>
                {
                    b.Navigation("UserPosts");
                });

            modelBuilder.Entity("dotnet_facebook.Models.DatabaseObjects.Posts.MainPost", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}