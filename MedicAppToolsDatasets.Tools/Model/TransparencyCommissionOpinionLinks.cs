using CsvHelper.Configuration.Attributes;

public class TransparencyCommissionOpinionLinks
{
    [Index(0)]
    [TypeConverter(typeof(ParseStringConverter))]
    public string HasDossierCode { get; set; } = string.Empty;

    [Index(1)]
    [TypeConverter(typeof(ParseUriConverter))]
    public Uri? CommissionOpinionLink { get; set; }
}