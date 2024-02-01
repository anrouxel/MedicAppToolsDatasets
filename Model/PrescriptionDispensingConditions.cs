using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

/// <summary>
/// Classe représentant les conditions de prescription et de délivrance d'un médicament.
/// </summary>
public class PrescriptionDispensingConditions
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
    /// Obtient ou définit la condition de prescription et de délivrance du médicament.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PrescriptionDispensingCondition { get; set; } = string.Empty;
}