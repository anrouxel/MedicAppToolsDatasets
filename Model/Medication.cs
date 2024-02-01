using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

/// <summary>
/// Classe représentant un médicament.
/// </summary>
public class Medication
{
    /// <summary>
    /// Obtient ou définit l'identifiant.
    /// </summary>
    [Ignore]
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Obtient ou définit le code CIS du médicament.
    /// </summary>
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le nom du médicament.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la forme pharmaceutique du médicament.
    /// </summary>
    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PharmaceuticalForm { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit les voies d'administration du médicament.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseListStringConverter))]
    public List<string> AdministrationRoutes { get; set; } = new();

    /// <summary>
    /// Obtient ou définit le statut de l'autorisation de mise sur le marché du médicament.
    /// </summary>
    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string MarketingAuthorizationStatus { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le type de procédure d'autorisation de mise sur le marché du médicament.
    /// </summary>
    [Index(5)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string MarketingAuthorizationProcedureType { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le statut de commercialisation du médicament.
    /// </summary>
    [Index(6)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CommercializationStatus { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la date de l'autorisation de mise sur le marché du médicament.
    /// </summary>
    [Index(7)]
    [TypeConverter(typeof(ParseDateOnlySlashConverter))]
    public DateOnly? MarketingAuthorizationDate { get; set; }

    /// <summary>
    /// Obtient ou définit le statut BDM du médicament.
    /// </summary>
    [Index(8)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string BdmStatus { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le numéro d'autorisation européen du médicament.
    /// </summary>
    [Index(9)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string EuropeanAuthorizationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit les détenteurs du médicament.
    /// </summary>
    [Index(10)]
    [TypeConverter(typeof(ParseListStringConverter))]
    public List<string> Holders { get; set; } = new();

    /// <summary>
    /// Obtient ou définit si le médicament est sous surveillance renforcée.
    /// </summary>
    [Index(11)]
    [TypeConverter(typeof(ParseBoolConverter))]
    public bool? EnhancedMonitoring { get; set; }

    /// <summary>
    /// Obtient ou définit les compositions du médicament.
    /// </summary>
    [Ignore]
    public List<MedicationComposition> MedicationCompositions { get; set; } = new();

    /// <summary>
    /// Obtient ou définit les présentations du médicament.
    /// </summary>
    [Ignore]
    public List<MedicationPresentation> MedicationPresentations { get; set; } = new();

    /// <summary>
    /// Obtient ou définit les groupes génériques du médicament.
    /// </summary>
    [Ignore]
    public List<GenericGroup> GenericGroups { get; set; } = new();

    /// <summary>
    /// Obtient ou définit les opinions SMR du médicament.
    /// </summary>
    [Ignore]
    public List<HasSmrOpinion> HasSmrOpinions { get; set; } = new();

    /// <summary>
    /// Obtient ou définit les opinions ASMR du médicament.
    /// </summary>
    [Ignore]
    public List<HasAsmrOpinion> HasAsmrOpinions { get; set; } = new();

    /// <summary>
    /// Obtient ou définit les informations importantes du médicament.
    /// </summary>
    [Ignore]
    public List<ImportantInformation> ImportantInformations { get; set; } = new();

    /// <summary>
    /// Obtient ou définit les conditions de délivrance du médicament.
    /// </summary>
    [Ignore]
    public List<PrescriptionDispensingConditions> PrescriptionDispensingConditions { get; set; } = new();

    /// <summary>
    /// Obtient ou définit les spécialités pharmaceutiques du médicament.
    /// </summary>
    [Ignore]
    public List<PharmaceuticalSpecialty> PharmaceuticalSpecialties { get; set; } = new();
}