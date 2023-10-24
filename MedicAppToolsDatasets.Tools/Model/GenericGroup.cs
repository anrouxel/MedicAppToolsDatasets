using CsvHelper.Configuration.Attributes;

public class GenericGroup
{
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string GenericGroupId { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string GenericGroupLabel { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(ParseGenericTypeConverter))]
    public GenericTypeInfo? GenericType { get; set; }

    [Index(4)]
    public int? SortNumber { get; set; }
}