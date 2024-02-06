using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

/// <summary>
/// Classe représentant des informations importantes.
/// </summary>
public class ImportantInformation
{
    /// <summary>
    /// Obtient ou définit l'identifiant.
    /// </summary>
    [Ignore]
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Obtient ou définit le code CIS.
    /// </summary>
    [Index(0)]
    public long CISCode { get; set; }

    /// <summary>
    /// Obtient ou définit la date de début des informations de sécurité.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseDateOnlyReverseDashConverter))]
    public DateOnly? SafetyInformationStartDate { get; set; }

    /// <summary>
    /// Obtient ou définit la date de fin des informations de sécurité.
    /// </summary>
    [Index(2)]
    [TypeConverter(typeof(ParseDateOnlyReverseDashConverter))]
    public DateOnly? SafetyInformationEndDate { get; set; }

    /// <summary>
    /// Obtient ou définit le lien vers les informations de sécurité.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string SafetyInformationLink { get; set; } = string.Empty;
}