using CsvHelper.Configuration.Attributes;

/// <summary>
/// Classe représentant un groupe générique.
/// </summary>
public class GenericGroup
{
    /// <summary>
    /// Obtient ou définit l'identifiant du groupe générique.
    /// </summary>
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string GenericGroupId { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le libellé du groupe générique.
    /// </summary>
    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string GenericGroupLabel { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le code CIS.
    /// </summary>
    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le type générique.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseGenericTypeConverter))]
    public string GenericType { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le numéro de tri.
    /// </summary>
    [Index(4)]
    public int? SortNumber { get; set; }
}