using CsvHelper.Configuration.Attributes;

public class MedicationComposition
{
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PharmaceuticalElementDesignation { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string SubstanceCode { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string SubstanceName { get; set; } = string.Empty;

    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string SubstanceDosage { get; set; } = string.Empty;

    [Index(5)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string DosageReference { get; set; } = string.Empty;

    [Index(6)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string ComponentNature { get; set; } = string.Empty;

    [Index(7)]
    public int? LinkNumber { get; set; }
}