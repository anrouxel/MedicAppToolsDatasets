using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

public class Medication
{
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string Name { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PharmaceuticalForm { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(ParseListStringConverter))]
    public List<string> AdministrationRoutes { get; set; } = new();

    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string MarketingAuthorizationStatus { get; set; } = string.Empty;

    [Index(5)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string MarketingAuthorizationProcedureType { get; set; } = string.Empty;

    [Index(6)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CommercializationStatus { get; set; } = string.Empty;

    [Index(7)]
    [TypeConverter(typeof(ParseDateOnlySlashConverter))]
    public DateOnly? MarketingAuthorizationDate { get; set; }

    [Index(8)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string BdmStatus { get; set; } = string.Empty;

    [Index(9)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string EuropeanAuthorizationNumber { get; set; } = string.Empty;

    [Index(10)]
    [TypeConverter(typeof(ParseListStringConverter))]
    public List<string> Holders { get; set; } = new();

    [Index(11)]
    [TypeConverter(typeof(ParseBoolConverter))]
    public bool? EnhancedMonitoring { get; set; }
}