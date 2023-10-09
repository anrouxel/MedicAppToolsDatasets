using CsvHelper.Configuration.Attributes;

public class ImportantInformation
{
    [Index(0)]
    [TypeConverter(typeof(StringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(DateOnlyReverseDashConverter))]
    public DateOnly? SafetyInformationStartDate { get; set; }

    [Index(2)]
    [TypeConverter(typeof(DateOnlyReverseDashConverter))]
    public DateOnly? SafetyInformationEndDate { get; set; }

    [Index(3)]
    [TypeConverter(typeof(StringConverter))]
    public string SafetyInformationLink { get; set; } = string.Empty;
}