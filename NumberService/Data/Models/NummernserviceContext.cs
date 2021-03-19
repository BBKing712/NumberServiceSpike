using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<Datentypen> Datentypens { get; set; }
        public virtual DbSet<Nummerdefinitionen> Nummerdefinitionens { get; set; }
        public virtual DbSet<Nummerdefinitionquellen> Nummerdefinitionquellens { get; set; }
        public virtual DbSet<Nummerinformationen> Nummerinformationens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MES-NB012;Database=Nummernservice;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Datentypen>(entity =>
            {
                entity.ToTable("Datentypen");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<Nummerdefinitionen>(entity =>
            {
                entity.ToTable("Nummerdefinitionen");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.QuelleBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ZielBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ZielDatentypenId).HasColumnName("ZielDatentypenID");

                entity.HasOne(d => d.ZielDatentypen)
                    .WithMany(p => p.Nummerdefinitionens)
                    .HasForeignKey(d => d.ZielDatentypenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionen__Datentypen_ID");
            });

            modelBuilder.Entity<Nummerdefinitionquellen>(entity =>
            {
                entity.ToTable("Nummerdefinitionquellen");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DatentypenId).HasColumnName("DatentypenID");

                entity.Property(e => e.NummerDefinitionenId).HasColumnName("NummerDefinitionenID");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Datentypen)
                    .WithMany(p => p.Nummerdefinitionquellens)
                    .HasForeignKey(d => d.DatentypenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionquellen_Datentypen_ID");

                entity.HasOne(d => d.NummerDefinitionen)
                    .WithMany(p => p.Nummerdefinitionquellens)
                    .HasForeignKey(d => d.NummerDefinitionenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionquellen_Nummerdefinitionen_ID");
            });

            modelBuilder.Entity<Nummerinformationen>(entity =>
            {
                entity.ToTable("Nummerinformationen");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.NummerdefinitionId).HasColumnName("NummerdefinitionID");

                entity.Property(e => e.Quelle).IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Nummerdefinition)
                    .WithMany(p => p.Nummerinformationens)
                    .HasForeignKey(d => d.NummerdefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerinformationen_NummerDefinitionen_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
