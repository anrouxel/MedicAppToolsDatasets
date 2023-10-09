using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

public class Medication
{
    [Index(0)]
    [TypeConverter(typeof(StringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(StringConverter))]
    public string Name { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(StringConverter))]
    public string PharmaceuticalForm { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(ListStringConverter))]
    public List<string> AdministrationRoutes { get; set; } = new();

    [Index(4)]
    [TypeConverter(typeof(StringConverter))]
    public string MarketingAuthorizationStatus { get; set; } = string.Empty;

    [Index(5)]
    [TypeConverter(typeof(StringConverter))]
    public string MarketingAuthorizationProcedureType { get; set; } = string.Empty;

    [Index(6)]
    [TypeConverter(typeof(StringConverter))]
    public string CommercializationStatus { get; set; } = string.Empty;

    [Index(7)]
    [TypeConverter(typeof(DateOnlySlashConverter))]
    public DateOnly MarketingAuthorizationDate { get; set; }

    [Index(8)]
    [TypeConverter(typeof(StringConverter))]
    public string BdmStatus { get; set; } = string.Empty;

    [Index(9)]
    [TypeConverter(typeof(StringConverter))]
    public string EuropeanAuthorizationNumber { get; set; } = string.Empty;

    [Index(10)]
    [TypeConverter(typeof(ListStringConverter))]
    public List<string> Holders { get; set; } = new();

    [Index(11)]
    [TypeConverter(typeof(BoolConverter))]
    public bool EnhancedMonitoring { get; set; }
}