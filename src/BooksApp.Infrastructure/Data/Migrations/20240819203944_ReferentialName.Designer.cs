﻿// <auto-generated />
using System;
using BooksApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PostsApp.Infrastructure.Data;

#nullable disable

namespace PostsApp.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240819203944_ReferentialName")]
    partial class ReferentialName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BooksGenres", b =>
                {
                    b.Property<Guid>("BooksId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GenresId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BooksId", "GenresId");

                    b.HasIndex("BookId");

                    b.HasIndex("GenresId");

                    b.ToTable("BooksGenres");
                });

            modelBuilder.Entity("PostsApp.Domain.Book.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CoverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferentialName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CoverId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("PostsApp.Domain.Bookshelf.Bookshelf", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferentialName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Bookshelves");
                });

            modelBuilder.Entity("PostsApp.Domain.Genre.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("PostsApp.Domain.Image.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("PostsApp.Domain.Review.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("PostsApp.Domain.Role.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6c427f5f-ebc3-4995-8574-53a620a42609"),
                            Name = "member"
                        },
                        new
                        {
                            Id = new Guid("b9ec54dc-01c3-4e43-83a7-b46dc0400102"),
                            Name = "author"
                        },
                        new
                        {
                            Id = new Guid("dbffd125-1660-4476-a55d-f2223114120f"),
                            Name = "moderator"
                        },
                        new
                        {
                            Id = new Guid("509c183d-17ff-4ae4-ace5-b3ed06b09db4"),
                            Name = "admin"
                        });
                });

            modelBuilder.Entity("PostsApp.Domain.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AvatarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AvatarId");

                    b.HasIndex("RoleId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BooksGenres", b =>
                {
                    b.HasOne("PostsApp.Domain.Book.Book", null)
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("PostsApp.Domain.Book.Book", null)
                        .WithMany()
                        .HasForeignKey("BooksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PostsApp.Domain.Genre.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PostsApp.Domain.Book.Book", b =>
                {
                    b.HasOne("PostsApp.Domain.User.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PostsApp.Domain.Image.Image", "Cover")
                        .WithMany()
                        .HasForeignKey("CoverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Cover");
                });

            modelBuilder.Entity("PostsApp.Domain.Bookshelf.Bookshelf", b =>
                {
                    b.HasOne("PostsApp.Domain.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.OwnsMany("PostsApp.Domain.Bookshelf.Entities.BookshelfBook", "BookshelfBooks", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("BookId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("BookshelfId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id");

                            b1.HasIndex("BookId");

                            b1.HasIndex("BookshelfId");

                            b1.ToTable("BookshelfBooks");

                            b1.HasOne("PostsApp.Domain.Book.Book", "Book")
                                .WithMany()
                                .HasForeignKey("BookId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("BookshelfId");

                            b1.Navigation("Book");
                        });

                    b.Navigation("BookshelfBooks");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PostsApp.Domain.Review.Review", b =>
                {
                    b.HasOne("PostsApp.Domain.Book.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("PostsApp.Domain.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PostsApp.Domain.User.User", b =>
                {
                    b.HasOne("PostsApp.Domain.Image.Image", "Avatar")
                        .WithMany()
                        .HasForeignKey("AvatarId");

                    b.HasOne("PostsApp.Domain.Role.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Avatar");

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
