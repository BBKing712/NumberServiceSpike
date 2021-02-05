using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Datenbank.Models
{
    public partial class NumberserviceContext : DbContext
    {
        public NumberserviceContext()
        {
        }

        public NumberserviceContext(DbContextOptions<NumberserviceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Datentyp> Datentyps { get; set; }
        public virtual DbSet<NummerDefinition> NummerDefinitions { get; set; }
        public virtual DbSet<NummerDefinitionQuelle> NummerDefinitionQuelles { get; set; }
        public virtual DbSet<NummerInformation> NummerInformations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MES-NB012;Database=Numberservice;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Datentyp>(entity =>
            {
                entity.ToTable("datentyp");

                entity.HasIndex(e => e.DatentypBezeichnung, "unique_datentyp")
                    .IsUnique();

                entity.Property(e => e.DatentypId).HasColumnName("datentyp_id");

                entity.Property(e => e.DatentypBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("datentyp_bezeichnung");
            });

            modelBuilder.Entity<NummerDefinition>(entity =>
            {
                entity.ToTable("nummer_definition");

                entity.HasIndex(e => e.NummerDefinitionBezeichnung, "Unique_nummer_definition")
                    .IsUnique();

                entity.Property(e => e.NummerDefinitionId).HasColumnName("nummer_definition_id");

                entity.Property(e => e.NummerDefinitionBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nummer_definition_bezeichnung");

                entity.Property(e => e.NummerDefinitionQuelleBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nummer_definition_quelle_bezeichnung");

                entity.Property(e => e.NummerDefinitionZielBezeichnung)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("nummer_definition_ziel_bezeichnung");

                entity.Property(e => e.NummerDefinitionZielDatentypId).HasColumnName("nummer_definition_ziel_datentyp_id");

                entity.HasOne(d => d.NummerDefinitionZielDatentyp)
                    .WithMany(p => p.NummerDefinitions)
                    .HasForeignKey(d => d.NummerDefinitionZielDatentypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nummer_definition_datentyp");
            });

            modelBuilder.Entity<NummerDefinitionQuelle>(entity =>
            {
                entity.ToTable("nummer_definition_quelle");

                entity.HasIndex(e => new { e.NummerDefinitionId, e.NummerDefinitionPos }, "Unique_nummer_definition_quelle_posperid")
                    .IsUnique();

                entity.Property(e => e.NummerDefinitionQuelleId).HasColumnName("nummer_definition_quelle_id");

                entity.Property(e => e.NummerDefinitionBezeichnung)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("nummer_definition_bezeichnung")
                    .IsFixedLength(true);

                entity.Property(e => e.NummerDefinitionDatentypId).HasColumnName("nummer_definition_datentyp_id");

                entity.Property(e => e.NummerDefinitionId).HasColumnName("nummer_definition_id");

                entity.Property(e => e.NummerDefinitionPos).HasColumnName("nummer_definition_pos");

                entity.HasOne(d => d.NummerDefinitionDatentyp)
                    .WithMany(p => p.NummerDefinitionQuelles)
                    .HasForeignKey(d => d.NummerDefinitionDatentypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nummer_definition_quelle_datentyp");
            });

            modelBuilder.Entity<NummerInformation>(entity =>
            {
                entity.ToTable("nummer_information");

                entity.Property(e => e.NummerInformationId).HasColumnName("nummer_information_id");

                entity.Property(e => e.NnmmerInformationQuelle)
                    .IsRequired()
                    .HasColumnName("Nnmmer_information_quelle");

                entity.Property(e => e.NummerDefinitionId).HasColumnName("nummer_definition_id");

                entity.Property(e => e.NummerInformationZiel)
                    .IsRequired()
                    .HasColumnName("nummer_information_ziel");

                entity.HasOne(d => d.NummerDefinition)
                    .WithMany(p => p.NummerInformations)
                    .HasForeignKey(d => d.NummerDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nummer_information_nummer_definition");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
