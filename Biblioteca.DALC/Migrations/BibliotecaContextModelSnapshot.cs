﻿// <auto-generated />
using System;
using Biblioteca.DALC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Biblioteca.DALC.Migrations
{
    [DbContext(typeof(BibliotecaContext))]
    partial class BibliotecaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Biblioteca.Model.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellidos");

                    b.Property<DateTime>("FechaNacimiento");

                    b.Property<string>("Nombres");

                    b.HasKey("Id");

                    b.ToTable("Autor");
                });

            modelBuilder.Entity("Biblioteca.Model.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descripcion");

                    b.Property<string>("Nombre");

                    b.HasKey("Id");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("Biblioteca.Model.Libro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AutorId");

                    b.Property<int?>("CategoriaId");

                    b.Property<string>("Nombre");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Libro");
                });

            modelBuilder.Entity("Biblioteca.Model.Libro", b =>
                {
                    b.HasOne("Biblioteca.Model.Autor", "Autor")
                        .WithMany()
                        .HasForeignKey("AutorId");

                    b.HasOne("Biblioteca.Model.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId");
                });
#pragma warning restore 612, 618
        }
    }
}
