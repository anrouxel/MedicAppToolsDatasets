using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

public class MedicationContext : DbContext
{
    public DbSet<Medication> Medications { get; set; }
    public DbSet<MedicationComposition> MedicationCompositions { get; set; }
    public DbSet<MedicationPresentation> MedicationPresentations { get; set; }
    public DbSet<GenericGroup> GenericGroups { get; set; }
    public DbSet<HasSmrOpinion> HasSmrOpinions { get; set; }
    public DbSet<HasAsmrOpinion> HasAsmrOpinions { get; set; }
    public DbSet<ImportantInformation> ImportantInformations { get; set; }
    public DbSet<PrescriptionDispensingConditions> PrescriptionDispensingConditions { get; set; }
    public DbSet<TransparencyCommissionOpinionLinks> TransparencyCommissionOpinionLinks { get; set; }
    public DbSet<PharmaceuticalSpecialty> PharmaceuticalSpecialties { get; set; }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medication>()
            .HasKey(c => c.CISCode);

        modelBuilder.Entity<MedicationComposition>()
            .HasKey(c => new { c.CISCode });
            
        modelBuilder.Entity<MedicationPresentation>()
            .HasKey(c => new { c.CISCode });

        modelBuilder.Entity<GenericGroup>()
            .HasKey(c => new { c.CISCode });

        modelBuilder.Entity<HasSmrOpinion>()
            .HasKey(c => new { c.CISCode, c.HasDossierCode });

        modelBuilder.Entity<HasAsmrOpinion>()
            .HasKey(c => new { c.CISCode, c.HasDossierCode });

        modelBuilder.Entity<ImportantInformation>()
            .HasKey(c => new { c.CISCode });

        modelBuilder.Entity<PrescriptionDispensingConditions>()
            .HasKey(c => new { c.CISCode });

        modelBuilder.Entity<TransparencyCommissionOpinionLinks>()
            .HasKey(c => new { c.HasDossierCode });
    }*/

    public string DbPath { get; }

    public MedicationContext(string path)
    {
        DbPath = path;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}