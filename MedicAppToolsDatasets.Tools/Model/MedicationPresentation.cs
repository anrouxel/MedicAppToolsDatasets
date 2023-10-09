using CsvHelper.Configuration.Attributes;

public class MedicationPresentation
{
    [Index(0)]
    [TypeConverter(typeof(StringConverter))]
    public string CISCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(StringConverter))]
    public string CIP7Code { get; set; } = string.Empty;

    [Index(2)]
    [TypeConverter(typeof(StringConverter))]
    public string PresentationLabel { get; set; } = string.Empty;

    [Index(3)]
    [TypeConverter(typeof(StringConverter))]
    public string PresentationStatus { get; set; } = string.Empty;

    [Index(4)]
    [TypeConverter(typeof(StringConverter))]
    public string PresentationCommercializationStatus { get; set; } = string.Empty;

    [Index(5)]
    [TypeConverter(typeof(DateOnlySlashConverter))]
    public DateOnly? CommercializationDeclarationDate { get; set; }

    [Index(6)]
    [TypeConverter(typeof(StringConverter))]
    public string CIP13Code { get; set; } = string.Empty;

    [Index(7)]
    [TypeConverter(typeof(BoolConverter))]
    public bool? ApprovalForCommunities { get; set; }

    [Index(8)]
    [TypeConverter(typeof(ListPourcentConverter))]
    public List<decimal> ReimbursementRates { get; set; } = new();

    [Index(9)]
    public decimal? PriceWithoutHonoraryInEuro { get; set; }

    [Index(10)]
    public decimal? PriceWithHonoraryInEuro { get; set; }

    [Index(11)]
    public decimal? PriceHonoraryInEuro { get; set; }

    [Index(12)]
    [TypeConverter(typeof(StringConverter))]
    public string ReimbursementIndications { get; set; } = string.Empty;
}