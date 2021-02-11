namespace API.Models
{
    using Microsoft.EntityFrameworkCore;

    public partial class NumberserviceContext : DbContext
    {
        public NumberserviceContext()
        {
        }

        public NumberserviceContext(DbContextOptions<NumberserviceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Datentyp> Datentyp { get; set; }

        public virtual DbSet<NummerDefinition> NummerDefinition { get; set; }

        public virtual DbSet<NummerDefinitionQuelle> NummerDefinitionQuelle { get; set; }

        public virtual DbSet<NummerInformation> NummerInformation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MES-NB012;Database=Numberservice;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Datentyp>(entity =>
            {
                entity.ToTable("datentyp");

                entity.HasIndex(e => e.DatentypBezeichnung)
                    .HasName("unique_datentyp_bezeichnung")
                    .IsUnique();

                entity.Property(e => e.DatentypId).HasColumnName("datentyp_id");

                entity.Property(e => e.DatentypBezeichnung)
                    .IsRequired()
                    .HasColumnName("datentyp_bezeichnung")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<NummerDefinition>(entity =>
            {
                entity.ToTable("nummer_definition");

                entity.HasIndex(e => e.NummerDefinitionBezeichnung)
                    .HasName("Unique_nummer_definition_bezeichnung")
                    .IsUnique();

                entity.Property(e => e.NummerDefinitionId).HasColumnName("nummer_definition_id");

                entity.Property(e => e.NummerDefinitionBezeichnung)
                    .IsRequired()
                    .HasColumnName("nummer_definition_bezeichnung")
                    .HasMaxLength(50);

                entity.Property(e => e.NummerDefinitionGuid)
    .HasColumnName("nummer_definition_guid")
    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.NummerDefinitionQuelleBezeichnung)
                    .IsRequired()
                    .HasColumnName("nummer_definition_quelle_bezeichnung")
                    .HasMaxLength(50);

                entity.Property(e => e.NummerDefinitionZielBezeichnung)
                    .IsRequired()
                    .HasColumnName("nummer_definition_ziel_bezeichnung")
                    .HasMaxLength(50);

                entity.Property(e => e.NummerDefinitionZielDatentypId).HasColumnName("nummer_definition_ziel_datentyp_id");

                entity.HasOne(d => d.NummerDefinitionZielDatentyp)
                    .WithMany(p => p.NummerDefinitionen)
                    .HasForeignKey(d => d.NummerDefinitionZielDatentypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nummer_definition_datentyp");
            });

            modelBuilder.Entity<NummerDefinitionQuelle>(entity =>
            {
                entity.ToTable("nummer_definition_quelle");

                entity.HasIndex(e => new { e.NummerDefinitionId, e.NummerDefinitionQuelleBezeichnung })
                    .HasName("Unique_nummer_definition_quelle_bezeichnungpernummer_definition_id");

                entity.HasIndex(e => new { e.NummerDefinitionId, e.NummerDefinitionQuellePos })
                    .HasName("Unique_nummer_definition_quelle_pospernummer_definition_id")
                    .IsUnique();

                entity.Property(e => e.NummerDefinitionQuelleId).HasColumnName("nummer_definition_quelle_id");

                entity.Property(e => e.NummerDefinitionId).HasColumnName("nummer_definition_id");

                entity.Property(e => e.NummerDefinitionQuelleBezeichnung)
                    .IsRequired()
                    .HasColumnName("nummer_definition_quelle_bezeichnung")
                    .HasMaxLength(50);

                entity.Property(e => e.NummerDefinitionQuelleDatentypId).HasColumnName("nummer_definition_quelle_datentyp_id");

                entity.Property(e => e.NummerDefinitionQuellePos).HasColumnName("nummer_definition_quelle_pos");

                entity.HasOne(d => d.NummerDefinitionQuelleDatentyp)
                    .WithMany(p => p.NummerDefinitionQuellen)
                    .HasForeignKey(d => d.NummerDefinitionQuelleDatentypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nummer_definition_quelle_datentyp");
            });

            modelBuilder.Entity<NummerInformation>(entity =>
            {
                entity.ToTable("nummer_information");

                entity.HasIndex(e => e.NummerInformationGuid)
                    .HasName("Unique_nummer_information_guid");

                entity.Property(e => e.NummerInformationId).HasColumnName("nummer_information_id");

                entity.Property(e => e.NnmmerInformationQuelle)
                    .IsRequired()
                    .HasColumnName("Nnmmer_information_quelle");

                entity.Property(e => e.NummerDefinitionId).HasColumnName("nummer_definition_id");

                entity.Property(e => e.NummerInformationGuid)
                    .HasColumnName("nummer_information_guid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.NummerInformationZiel)
                    .IsRequired()
                    .HasColumnName("nummer_information_ziel");

                entity.HasOne(d => d.NummerDefinition)
                    .WithMany(p => p.NummerInformationen)
                    .HasForeignKey(d => d.NummerDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_nummer_information_nummer_definition");
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
