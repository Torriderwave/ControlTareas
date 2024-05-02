using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ControlTareas.Models;

public partial class DbcontrolTContext : DbContext
{
    public DbcontrolTContext()
    {
    }

    public DbcontrolTContext(DbContextOptions<DbcontrolTContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__estado__FBB0EDC167EAEDA0");

            entity.ToTable("estado");

            entity.Property(e => e.Estado1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Estado");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.IdTarea).HasName("PK__Tareas__EADE90986CEC3942");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fecha)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.oEstado).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("fk_estado");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97F6AA4215");

            entity.ToTable("Usuario");

            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
