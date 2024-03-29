using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

/// <summary>
/// Classe représentant un groupe générique.
/// </summary>
public class GenericGroup
{
    /// <summary>
    /// Obtient ou définit l'identifiant.
    /// </summary>
    [Ignore]
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du groupe générique.
    /// </summary>
    [Index(0)]
    public long GenericGroupId { get; set; }

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
    public long CISCode { get; set; }

    /// <summary>
    /// Obtient ou définit le type générique.
    /// </summary>
    [Index(3)]
    public int GenericType { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé du type générique.
    /// </summary>
    [Index(3)]
    [TypeConverter(typeof(ParseGenericTypeConverter))]
    public string GenericName { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le numéro de tri.
    /// </summary>
    [Index(4)]
    public int? SortNumber { get; set; }
}