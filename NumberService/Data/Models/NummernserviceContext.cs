

namespace Data.Models
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    public partial class NummernserviceContext : DbContext
    {
        public NummernserviceContext()
        {
        }

        public NummernserviceContext(DbContextOptions<NummernserviceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Datentyp> Datentypen { get; set; }
        public virtual DbSet<NummerDefinition> Nummerdefinitionen { get; set; }
        public virtual DbSet<NummerDefinitionQuelle> Nummerdefinitionquellen { get; set; }
        public virtual DbSet<NummerInformation> Nummerinformationen { get; set; }

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
            modelBuilder.Entity<Datentyp>(entity =>
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

            modelBuilder.Entity<NummerDefinition>(entity =>
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

                entity.HasOne(d => d.ZielDatentyp)
                    .WithMany(p => p.NummerDefinitionen)
                    .HasForeignKey(d => d.ZielDatentypenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionen__Datentypen_ID");
            });

            modelBuilder.Entity<NummerDefinitionQuelle>(entity =>
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

                entity.HasOne(d => d.Datentyp)
                    .WithMany(p => p.NummerDefinitionQuellen)
                    .HasForeignKey(d => d.DatentypenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionquellen_Datentypen_ID");

                entity.HasOne(d => d.NummerDefinition)
                    .WithMany(p => p.NummerDefinitionQuellen)
                    .HasForeignKey(d => d.NummerdefinitionenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerdefinitionquellen_Nummerdefinitionen_ID");
            });

            modelBuilder.Entity<NummerInformation>(entity =>
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

                entity.HasOne(d => d.NummerDefinition)
                    .WithMany(p => p.NummerInformationen)
                    .HasForeignKey(d => d.NummerdefinitionenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nummerinformationen_NummerDefinitionen_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
