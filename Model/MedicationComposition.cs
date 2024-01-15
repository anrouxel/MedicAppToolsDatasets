using CsvHelper.Configuration.Attributes;

/// <summary>
/// Classe représentant la composition d'un médicament.
/// </summary>
public class MedicationComposition
{
    /// <summary>
    /// Obtient ou définit le code CIS du médicament.
    /// </summary>
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la désignation de l'élément pharmaceutique.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PharmaceuticalElementDesignation { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le code de la substance.
    /// </summary>
    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string SubstanceCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le nom de la substance.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string SubstanceName { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le dosage de la substance.
    /// </summary>
    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string SubstanceDosage { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la référence du dosage.
    /// </summary>
    [Index(5)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string DosageReference { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la nature du composant.
    /// </summary>
    [Index(6)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string ComponentNature { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le numéro de lien.
    /// </summary>
    [Index(7)]
    public int? LinkNumber { get; set; }
}