using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

/// <summary>
/// Represents pharmaceutical specialties information from the ANSM's "Disponibilité des produits de santé" section.
/// </summary>
public class PharmaceuticalSpecialty
{
    /// <summary>
    /// Obtient ou définit l'identifiant.
    /// </summary>
    [Ignore]
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Code CIS (Code Identifiant de Spécialité)
    /// </summary>
    [Index(0)]
    public long CISCode { get; set; }

    /// <summary>
    /// Code CIP13 (Code Identifiant de Présentation à 13 chiffres)
    /// This column will not be filled if all commercialized presentations of a pharmaceutical specialty are concerned.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string Cip13Code { get; set; } = string.Empty;

    [Index(2)]
    public int StatusCode { get; set; }

    /// <summary>
    /// Label of the status based on the StatusCode values.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string StatusLabel { get; set; } = string.Empty;

    /// <summary>
    /// Date of the status start (format JJ/MM/AAAA).
    /// For records before 06/10/2023: the date will be the update date, not the start date.
    /// </summary>
    [Index(4)]
    [TypeConverter(typeof(ParseDateOnlySlashConverter))]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// Date of the update on the ANSM site (format JJ/MM/AAAA).
    /// </summary>
    [Index(5)]
    [TypeConverter(typeof(ParseDateOnlySlashConverter))]
    public DateOnly? UpdateDate { get; set; }

    /// <summary>
    /// Date of the product's return to the market (format JJ/MM/AAAA).
    /// </summary>
    [Index(6)]
    [TypeConverter(typeof(ParseDateOnlySlashConverter))]
    public DateOnly? ReturnToDate { get; set; }

    /// <summary>
    /// Link to the ANSM site page.
    /// </summary>
    [Index(7)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string AnsmSiteLink { get; set; } = string.Empty;
}
