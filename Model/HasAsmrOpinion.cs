using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

/// <summary>
/// Classe représentant une opinion sur l'ASMR (Amélioration du Service Médical Rendu).
/// </summary>
public class HasAsmrOpinion
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
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le code du dossier.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string HasDossierCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la raison de l'évaluation.
    /// </summary>
    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string EvaluationReason { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la date de l'opinion de la Commission de la Transparence.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseDateOnlyReverseConverter))]
    public DateOnly? TransparencyCommissionOpinionDate { get; set; }

    /// <summary>
    /// Obtient ou définit la valeur de l'ASMR.
    /// </summary>
    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string AsmrValue { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le libellé de l'ASMR.
    /// </summary>
    [Index(5)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string AsmrLabel { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit les liens vers les opinions de la Commission de la Transparence.
    /// </summary>
    [Ignore]
    public List<TransparencyCommissionOpinionLinks> TransparencyCommissionOpinionLinks { get; set; } = new();
}