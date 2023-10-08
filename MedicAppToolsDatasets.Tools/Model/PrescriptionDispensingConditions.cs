using CsvHelper.Configuration.Attributes;

public class PrescriptionDispensingConditions
{
    [Index(0)]
    [TypeConverter(typeof(StringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(StringConverter))]
    public string PrescriptionDispensingCondition { get; set; } = string.Empty;
}