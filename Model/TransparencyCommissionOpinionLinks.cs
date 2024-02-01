using CsvHelper.Configuration.Attributes;
using System;
using System.Text.Json.Serialization;

/// <summary>
/// Classe représentant les liens vers les avis de la Commission de la Transparence.
/// </summary>
public class TransparencyCommissionOpinionLinks
{
    /// <summary>
    /// Obtient ou définit l'identifiant.
    /// </summary>
    [Ignore]
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Obtient ou définit le code indiquant si le dossier a été examiné par la Commission de la Transparence.
    /// </summary>
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string HasDossierCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le lien vers l'avis de la Commission de la Transparence.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseUriConverter))]
    public Uri? CommissionOpinionLink { get; set; }
}