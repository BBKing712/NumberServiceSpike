﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Data.Models
{
    public partial class NummernserviceContext : DbContext
    {
        public NummernserviceContext()
        {
        }

        public NummernserviceContext(DbContextOptions<NummernserviceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Datentypen> Datentypen { get; set; }
        public virtual DbSet<Nummerdefinitionen> Nummerdefinitionen { get; set; }
        public virtual DbSet<Nummerdefinitionquellen> Nummerdefinitionquellen { get; set; }
        public virtual DbSet<Nummerinformationen> Nummerinformationen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MES-NB012;Database=Nummernservice;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Datentypen>(entity =>
            {
                entity.HasIndex(e => e.Bezeichnung)
                    .HasName("UQ_Datentypen_Bezeichnung")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AktualisiertAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ErstelltAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<Nummerdefinitionen>(entity =>
            {
                entity.HasIndex(e => e.Bezeichnung)
                    .HasName("UQ_Nummerdefinitionen_Bezeichnung")
                    .IsUnique();

                entity.HasIndex(e => e.Guid)
                    .HasName("UQ_Nummerdefinitionen_Guid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AktualisiertAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ErstelltAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.QuelleBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ZielBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ZielDatentypenId).HasColumnName("ZielDatentypenID");

                entity.HasOne(d => d.ZielDatentypen)
                    .WithMany(p => p.Nummerdefinitionen)
                    .HasForeignKey(d => d.ZielDatentypenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionen__Datentypen_ID");
            });

            modelBuilder.Entity<Nummerdefinitionquellen>(entity =>
            {
                entity.HasIndex(e => new { e.NummerdefinitionenId, e.Bezeichnung })
                    .HasName("UQC_Nummerdefinitionquellen_NummerdefinitionenID_Bezeichnung")
                    .IsUnique();

                entity.HasIndex(e => new { e.NummerdefinitionenId, e.Position })
                    .HasName("UQC_Nummerdefinitionquellen_NummerdefinitionenID_Position")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AktualisiertAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DatentypenId).HasColumnName("DatentypenID");

                entity.Property(e => e.ErstelltAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.NummerdefinitionenId).HasColumnName("NummerdefinitionenID");

                entity.HasOne(d => d.Datentypen)
                    .WithMany(p => p.Nummerdefinitionquellen)
                    .HasForeignKey(d => d.DatentypenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionquellen_Datentypen_ID");

                entity.HasOne(d => d.Nummerdefinitionen)
                    .WithMany(p => p.Nummerdefinitionquellen)
                    .HasForeignKey(d => d.NummerdefinitionenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionquellen_Nummerdefinitionen_ID");
            });

            modelBuilder.Entity<Nummerinformationen>(entity =>
            {
                entity.HasIndex(e => e.Guid)
                    .HasName("UQ_Nummerinformationen_Guid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AktualisiertAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ErstelltAm)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.NummerdefinitionenId).HasColumnName("NummerdefinitionenID");

                entity.Property(e => e.Quelle).IsRequired();

                entity.HasOne(d => d.Nummerdefinitionen)
                    .WithMany(p => p.Nummerinformationen)
                    .HasForeignKey(d => d.NummerdefinitionenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerinformationen_NummerDefinitionen_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
