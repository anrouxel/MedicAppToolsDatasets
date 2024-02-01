using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

/// <summary>
/// Classe représentant une présentation de médicament.
/// </summary>
public class MedicationPresentation
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
    /// Obtient ou définit le code CIP7 de la présentation.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CIP7Code { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le libellé de la présentation.
    /// </summary>
    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PresentationLabel { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le statut de la présentation.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PresentationStatus { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le statut de commercialisation de la présentation.
    /// </summary>
    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PresentationCommercializationStatus { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la date de déclaration de commercialisation de la présentation.
    /// </summary>
    [Index(5)]
    [TypeConverter(typeof(ParseDateOnlySlashConverter))]
    public DateOnly? CommercializationDeclarationDate { get; set; }

    /// <summary>
    /// Obtient ou définit le code CIP13 de la présentation.
    /// </summary>
    [Index(6)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CIP13Code { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit si la présentation est approuvée pour les communautés.
    /// </summary>
    [Index(7)]
    [TypeConverter(typeof(ParseBoolConverter))]
    public bool? ApprovalForCommunities { get; set; }

    /// <summary>
    /// Obtient ou définit les taux de remboursement de la présentation.
    /// </summary>
    [Index(8)]
    [TypeConverter(typeof(ParseListPourcentConverter))]
    public List<decimal> ReimbursementRates { get; set; } = new();

    /// <summary>
    /// Obtient ou définit le prix sans honoraire en euro de la présentation.
    /// </summary>
    [Index(9)]
    [TypeConverter(typeof(ParseDecimalConverter))]
    public decimal? PriceWithoutHonoraryInEuro { get; set; }

    /// <summary>
    /// Obtient ou définit le prix avec honoraire en euro de la présentation.
    /// </summary>
    [Index(10)]
    [TypeConverter(typeof(ParseDecimalConverter))]
    public decimal? PriceWithHonoraryInEuro { get; set; }

    /// <summary>
    /// Obtient ou définit le prix de l'honoraire en euro de la présentation.
    /// </summary>
    [Index(11)]
    [TypeConverter(typeof(ParseDecimalConverter))]
    public decimal? PriceHonoraryInEuro { get; set; }

    /// <summary>
    /// Obtient ou définit les indications de remboursement de la présentation.
    /// </summary>
    [Index(12)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string ReimbursementIndications { get; set; } = string.Empty;
}