using CsvHelper.Configuration.Attributes;

public class MedicationPresentation
{
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CIP7Code { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PresentationLabel { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PresentationStatus { get; set; } = string.Empty;

    [Index(4)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string PresentationCommercializationStatus { get; set; } = string.Empty;

    [Index(5)]
    [TypeConverter(typeof(ParseDateOnlySlashConverter))]
    public DateOnly? CommercializationDeclarationDate { get; set; }

    [Index(6)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string CIP13Code { get; set; } = string.Empty;

    [Index(7)]
    [TypeConverter(typeof(ParseBoolConverter))]
    public bool? ApprovalForCommunities { get; set; }

    [Index(8)]
    [TypeConverter(typeof(ParseListPourcentConverter))]
    public List<decimal> ReimbursementRates { get; set; } = new();

    [Index(9)]
    [TypeConverter(typeof(ParseDecimalConverter))]
    public decimal? PriceWithoutHonoraryInEuro { get; set; }

    [Index(10)]
    [TypeConverter(typeof(ParseDecimalConverter))]
    public decimal? PriceWithHonoraryInEuro { get; set; }

    [Index(11)]
    [TypeConverter(typeof(ParseDecimalConverter))]
    public decimal? PriceHonoraryInEuro { get; set; }

    [Index(12)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string ReimbursementIndications { get; set; } = string.Empty;
}